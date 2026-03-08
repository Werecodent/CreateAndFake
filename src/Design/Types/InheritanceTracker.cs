using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CreateAndFake.Design.Extensions;

namespace CreateAndFake.Design.Types;

/// <summary>Finds all parents (base classes/interfaces) for <see cref="Type"/>s.</summary>
public sealed class InheritanceTracker : ITypeSupporter
{
    /// <summary>Every possible specific type.</summary>
    private static readonly FrozenSet<Type> _AllTypesFromAllAssemblies = FindAllAssemblies()
        .Where(a => !a.ReflectionOnly)
        .Where(a => !a.IsDynamic)
        .SelectMany(TypeDescriber.FindLoadedTypes)
        .ToFrozenSet();

    /// <summary>Prevents concurrency issues for <see cref="_InheritCache"/>.</summary>
    private static readonly Lock _Lock = new();

    /// <summary>Caches every parent inherited per <see cref="Type"/>.</summary>
    private static readonly Dictionary<Type, InheritanceTracker> _InheritCache = [];

    /// <summary>Associates no parents for <see langword="null"/>.</summary>
    private static readonly InheritanceTracker _NullDescriber = new(null, []);

    /// <summary>Finds or loads inheritance data for <typeparamref name="T"/>.</summary>
    /// <typeparam name="T">The <see cref="Type"/> to find inheritance for.</typeparam>
    /// <returns>The found/loaded inheritance data.</returns>
    public static InheritanceTracker For<T>()
    {
        return For(typeof(T));
    }

    /// <summary>Finds or loads inheritance data for the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find inheritance for.</param>
    /// <returns>The found/loaded inheritance data.</returns>
    public static InheritanceTracker For(Type? type)
    {
        if (type == null)
        {
            return _NullDescriber;
        }

        InheritanceTracker? describer;
        lock (_Lock)
        {
            if (!_InheritCache.TryGetValue(type, out describer))
            {
                describer = _InheritCache[type] = new(type, FindParentInheritance(type));
            }
        }
        return describer;
    }

    /// <inheritdoc/>
    public Type? SupportedType { get; }

    /// <summary>All found <see cref="Type"/>s the <see cref="SupportedType"/> inherits.</summary>
    public IEnumerable<Type> InheritedTypes { get; }

    /// <summary>All found <see cref="Type"/>s inheriting the <see cref="SupportedType"/>.</summary>
    public IEnumerable<Type> SubTypes => _subTypes.Value;

    /// <summary>All found properties on the <see cref="SupportedType"/>.</summary>
    public IEnumerable<PropertyInfo> AllProperties => _allProperties.Value;

    /// <summary>All found fields on the <see cref="SupportedType"/>.</summary>
    public IEnumerable<FieldInfo> AllFields => _allFields.Value;

    /// <summary>All found constructors on the <see cref="SupportedType"/>.</summary>
    public IEnumerable<ConstructorInfo> AllConstructors => _allConstructors.Value;

    /// <summary>All found factories on the <see cref="SupportedType"/>.</summary>
    public IEnumerable<MethodInfo> AllFactories => _allFactories.Value;

    /// <inheritdoc cref="SubTypes"/>
    private readonly Lazy<FrozenSet<Type>> _subTypes;

    /// <inheritdoc cref="AllProperties"/>
    private readonly Lazy<ImmutableHashSet<PropertyInfo>> _allProperties;

    /// <inheritdoc cref="AllFields"/>
    private readonly Lazy<ImmutableHashSet<FieldInfo>> _allFields;

    /// <inheritdoc cref="AllConstructors"/>
    private readonly Lazy<ImmutableHashSet<ConstructorInfo>> _allConstructors;

    /// <inheritdoc cref="AllFactories"/>
    private readonly Lazy<ImmutableHashSet<MethodInfo>> _allFactories;

    /// <summary><inheritdoc cref="InheritanceTracker"/></summary>
    /// <param name="type"><inheritdoc cref="SupportedType" path="/summary"/></param>
    /// <param name="parents"><inheritdoc cref="InheritedTypes" path="/summary"/></param>
    private InheritanceTracker(Type? type, IEnumerable<Type> parents)
    {
        SupportedType = type;
        InheritedTypes = parents.ToFrozenSet();
        _subTypes = new(() => [.. FindLoadedChildren(type)]);
        _allFields = new(() => [.. FindAllFields(type)]);
        _allProperties = new(() => [.. FindAllProperties(type)]);
        _allConstructors = new(() => [.. FindAllConstructors(type)]);
        _allFactories = new(() => [.. FindAllFactories(type)]);
    }

    /// <summary>
    ///     Checks if <typeparamref name="T"/> is a base <see langword="class"/>
    ///     or <see langword="interface"/> for the <see cref="SupportedType"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     Potential base <see langword="class"/>/<see langword="interface"/>
    ///     for the <see cref="SupportedType"/>.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="SupportedType"/> inherits
    ///     <typeparamref name="T"/>, <see langword="false"/> otherwise.
    /// </returns>
    public bool Inherits<T>()
    {
        return Inherits(typeof(T));
    }

    /// <summary>
    ///     Checks if <paramref name="parent"/> is a base <see langword="class"/>
    ///     or <see langword="interface"/> for the <see cref="SupportedType"/>.
    /// </summary>
    /// <param name="parent">
    ///     Potential base <see langword="class"/>/<see langword="interface"/>
    ///     for the <see cref="SupportedType"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="SupportedType"/> inherits
    ///     <paramref name="parent"/>, <see langword="false"/> otherwise.
    /// </returns>
    public bool Inherits([NotNullWhen(true)] Type? parent)
    {
        return parent != null
            && InheritedTypes.Contains(Nullable.GetUnderlyingType(parent) ?? parent);
    }

    /// <summary>
    ///     Finds every non-<see langword="abstract"/> <see langword="class"/> inheriting
    ///     the <see cref="SupportedType"/> in its defined <see cref="Assembly"/>.
    /// </summary>
    /// <inheritdoc cref="FindLoadedSubclasses(AssemblyName)"/>
    public IEnumerable<Type> FindLocalSubclasses()
    {
        AssemblyName assembly = Assembly.GetCallingAssembly().GetName();
        return FindLoadedSubclasses(assembly).Where(t => SupportedType?.Assembly == t.Assembly);
    }

    /// <inheritdoc cref="FindLoadedSubclasses(AssemblyName)"/>
    public IEnumerable<Type> FindLoadedSubclasses()
    {
        return FindLoadedSubclasses(Assembly.GetCallingAssembly().GetName());
    }

    /// <summary>
    ///     Finds every non-<see langword="abstract"/> <see langword="class"/>
    ///     inheriting the <see cref="SupportedType"/> in all loaded assemblies.
    /// </summary>
    /// <param name="assembly">Accessing assembly to check visibility access for.</param>
    /// <returns>The found creatable subclasses.</returns>
    /// <remarks>
    ///     Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to access its <see langword="internal"/> types for the test framework.
    /// </remarks>
    private IEnumerable<Type> FindLoadedSubclasses(AssemblyName assembly)
    {
        return SubTypes
            .Where(t => !t.IsAbstract)
            .Where(t => !t.IsGenericTypeDefinition)
            .Where(t => TypeDescriber.IsVisible(t, assembly));
    }

    /// <inheritdoc cref="IsMutable(AssemblyName)"/>
    public bool IsMutable()
    {
        return IsMutable(Assembly.GetCallingAssembly().GetName());
    }

    /// <summary>
    ///     Determines if the <see cref="SupportedType"/> has any modifiable properties/fields.
    /// </summary>
    /// <inheritdoc cref="TypeDescriber.IsVisible(Type?, AssemblyName)"/>
    private bool IsMutable(AssemblyName assembly)
    {
        return GetMutableProperties(assembly).Any() || GetMutableFields(assembly).Any();
    }

    /// <summary>
    ///     Determines if the <see cref="SupportedType"/> has any
    ///     properties/fields only settable via a constructor.
    /// </summary>
    /// <remarks>Beware that this does not always mean the value is changeable.</remarks>
    public bool HasInitializableOnlyState()
    {
        return AllFields.Any(f => f.IsInitOnly && !f.IsLiteral)
            && AllConstructors.Any(c => c.GetParameters().Length > 0);
    }

    /// <inheritdoc cref="GetReadableMutableProperties(AssemblyName)"/>
    public IEnumerable<PropertyInfo> GetReadableMutableProperties()
    {
        return GetReadableMutableProperties(Assembly.GetCallingAssembly().GetName());
    }

    /// <inheritdoc cref="GetMutableProperties(AssemblyName)"/>
    private IEnumerable<PropertyInfo> GetReadableMutableProperties(AssemblyName assembly)
    {
        bool nonPublic = TypeDescriber.InternalsAreVisible(SupportedType, assembly);
        return GetMutableProperties(assembly)
            .Where(p =>
            {
                MethodInfo? getMethod = p.GetGetMethod(nonPublic);
                return getMethod != null && (getMethod.IsPublic || getMethod.IsAssembly);
            });
    }

    /// <summary>
    ///     Finds <see langword="public"/> instance properties on the <see cref="SupportedType"/>.
    /// </summary>
    /// <returns>All found properties on the <see cref="Type"/>.</returns>
    /// <remarks>Includes inherited <see langword="public"/> properties.</remarks>
    public IEnumerable<PropertyInfo> GetPublicProperties()
    {
        return SupportedType?.GetProperties() ?? [];
    }

    /// <inheritdoc cref="GetMutableProperties(AssemblyName)"/>
    public IEnumerable<PropertyInfo> GetMutableProperties()
    {
        return GetMutableProperties(Assembly.GetCallingAssembly().GetName());
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     instance properties on the <see cref="SupportedType"/> that can be written to.
    /// </summary>
    /// <inheritdoc cref="GetVisibleProperties(AssemblyName)"/>
    private IEnumerable<PropertyInfo> GetMutableProperties(AssemblyName assembly)
    {
        bool nonPublic = TypeDescriber.InternalsAreVisible(SupportedType, assembly);
        return AllProperties.Where(p =>
        {
            MethodInfo? setMethod = p.GetSetMethod(nonPublic);
            return setMethod != null && (setMethod.IsPublic || setMethod.IsAssembly);
        });
    }

    /// <inheritdoc cref="GetVisibleProperties(AssemblyName)"/>
    public IEnumerable<PropertyInfo> GetVisibleProperties()
    {
        return GetVisibleProperties(Assembly.GetCallingAssembly().GetName());
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     instance properties on the <see cref="SupportedType"/>.
    /// </summary>
    /// <param name="assembly">
    ///     Name of the <see cref="Assembly"/> to determine visibility for.
    /// </param>
    /// <remarks>
    ///     Finds <see langword="internal"/> properties only if they are visible
    ///     to the calling method's assembly. Mark an <see cref="Assembly"/> with
    ///     <c>InternalsVisibleTo("CreateAndFake")</c> to access its
    ///     <see langword="internal"/> properties for the test framework.
    /// </remarks>
    /// <inheritdoc cref="FindAllProperties"/>
    private IEnumerable<PropertyInfo> GetVisibleProperties(AssemblyName assembly)
    {
        if (TypeDescriber.InternalsAreVisible(SupportedType, assembly))
        {
            return AllProperties.Where(p =>
            {
                MethodInfo? getMethod = p.GetGetMethod(true);
                MethodInfo? setMethod = p.GetSetMethod(true);
                return (getMethod != null && (getMethod.IsPublic || getMethod.IsAssembly))
                    || (setMethod != null && (setMethod.IsPublic || setMethod.IsAssembly));
            });
        }
        else
        {
            return SupportedType?.GetProperties() ?? [];
        }
    }

    /// <summary>
    ///     Finds <see langword="public"/> instance fields on the <see cref="SupportedType"/>.
    /// </summary>
    /// <returns>All found fields on the <see cref="Type"/>.</returns>
    /// <remarks>Includes inherited <see langword="public"/> fields.</remarks>
    public IEnumerable<FieldInfo> GetPublicFields()
    {
        return SupportedType?.GetFields(BindingFlags.Instance | BindingFlags.Public) ?? [];
    }

    /// <inheritdoc cref="GetMutableFields(AssemblyName)"/>
    public IEnumerable<FieldInfo> GetMutableFields()
    {
        return GetMutableFields(Assembly.GetCallingAssembly().GetName());
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     instance fields on the <see cref="SupportedType"/> that can be written to.
    /// </summary>
    /// <inheritdoc cref="GetVisibleFields(AssemblyName)"/>
    private IEnumerable<FieldInfo> GetMutableFields(AssemblyName assembly)
    {
        return GetVisibleFields(assembly).Where(f => !f.IsInitOnly && !f.IsLiteral);
    }

    /// <inheritdoc cref="GetVisibleFields(AssemblyName)"/>
    public IEnumerable<FieldInfo> GetVisibleFields()
    {
        return GetVisibleFields(Assembly.GetCallingAssembly().GetName());
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     instance fields on the <see cref="SupportedType"/>.
    /// </summary>
    /// <param name="assembly">
    ///     Name of the <see cref="Assembly"/> to determine visibility for.
    /// </param>
    /// <remarks>
    ///     Finds <see langword="internal"/> fields only if they are visible to the calling method's
    ///     assembly. Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to access its <see langword="internal"/> fields for the test framework.
    /// </remarks>
    /// <inheritdoc cref="FindAllFields"/>
    private IEnumerable<FieldInfo> GetVisibleFields(AssemblyName assembly)
    {
        if (TypeDescriber.InternalsAreVisible(SupportedType, assembly))
        {
            return AllFields.Where(f => f.IsPublic || f.IsAssembly);
        }
        else
        {
            return AllFields.Where(f => f.IsPublic);
        }
    }

    /// <summary>
    ///     Finds <see langword="public"/> constructors on the <see cref="SupportedType"/>.
    /// </summary>
    /// <returns>All found constructors on the <see cref="Type"/>.</returns>
    /// <remarks>Includes inherited <see langword="public"/> constructors.</remarks>
    public IEnumerable<ConstructorInfo> GetPublicConstructors()
    {
        return SupportedType?.GetConstructors(BindingFlags.Instance | BindingFlags.Public) ?? [];
    }

    /// <inheritdoc cref="GetVisibleConstructors(AssemblyName)"/>
    public IEnumerable<ConstructorInfo> GetVisibleConstructors()
    {
        return GetVisibleConstructors(Assembly.GetCallingAssembly().GetName());
    }

    /// <summary>
    ///     Finds <see langword="public"/> static methods
    ///     that create the <see cref="SupportedType"/>.
    /// </summary>
    /// <returns>All found factories on the <see cref="Type"/>.</returns>
    public IEnumerable<MethodInfo> GetPublicFactories()
    {
        return SupportedType
                ?.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(m => m.ReturnType.Inherits(SupportedType))
            ?? [];
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     constructors on the <see cref="SupportedType"/>.
    /// </summary>
    /// <param name="assembly">
    ///     Name of the <see cref="Assembly"/> to determine visibility for.
    /// </param>
    /// <remarks>
    ///     Finds <see langword="internal"/> constructors only if they are visible
    ///     to the calling method's assembly. Mark an <see cref="Assembly"/> with
    ///     <c>InternalsVisibleTo("CreateAndFake")</c> to access its
    ///     <see langword="internal"/> constructors for the test framework.
    /// </remarks>
    /// <inheritdoc cref="FindAllConstructors"/>
    private IEnumerable<ConstructorInfo> GetVisibleConstructors(AssemblyName assembly)
    {
        if (TypeDescriber.InternalsAreVisible(SupportedType, assembly))
        {
            return AllConstructors.Where(c => c.IsPublic || c.IsAssembly);
        }
        else
        {
            return AllConstructors.Where(c => c.IsPublic);
        }
    }

    /// <inheritdoc cref="GetVisibleFactories(AssemblyName)"/>
    public IEnumerable<MethodInfo> GetVisibleFactories()
    {
        return GetVisibleFactories(Assembly.GetCallingAssembly().GetName());
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     static methods that create the <see cref="SupportedType"/>.
    /// </summary>
    /// <param name="assembly">
    ///     Name of the <see cref="Assembly"/> to determine visibility for.
    /// </param>
    /// <remarks>
    ///     Finds <see langword="internal"/> factories only if they are visible
    ///     to the calling method's assembly. Mark an <see cref="Assembly"/> with
    ///     <c>InternalsVisibleTo("CreateAndFake")</c> to access its
    ///     <see langword="internal"/> factories for the test framework.
    /// </remarks>
    /// <inheritdoc cref="FindAllFactories"/>
    private IEnumerable<MethodInfo> GetVisibleFactories(AssemblyName assembly)
    {
        if (TypeDescriber.InternalsAreVisible(SupportedType, assembly))
        {
            return AllFactories.Where(c => c.IsPublic || c.IsAssembly);
        }
        else
        {
            return AllFactories.Where(c => c.IsPublic);
        }
    }

    /// <summary>
    ///     Finds every child <see cref="Type"/> inheriting
    ///     the <paramref name="type"/> in all loaded assemblies.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to find subclasses for.</param>
    /// <returns>The found subclasses.</returns>
    private static IEnumerable<Type> FindLoadedChildren(Type? type)
    {
        return (type == null) ? [] : _AllTypesFromAllAssemblies.Where(t => t.Inherits(type));
    }

    /// <summary>Finds every <see cref="Type"/> that the <paramref name="type"/> inherits.</summary>
    /// <param name="type">The <see cref="Type"/> to find base classes/interfaces for.</param>
    /// <returns>All found base classes/interfaces.</returns>
    private static HashSet<Type> FindParentInheritance(Type type)
    {
        HashSet<Type> foundParents = [type];
        Stack<Type> sourceTypes = new(foundParents);

        void attemptAdd(Type? newType)
        {
            if (newType != null && foundParents.Add(newType))
            {
                sourceTypes.Push(newType);
            }
        }

        while (sourceTypes.Count > 0)
        {
            Type source = sourceTypes.Pop();
            if (source.IsGenericType)
            {
                attemptAdd(source.GetGenericTypeDefinition());
            }
            foreach (Type parent in source.GetInterfaces())
            {
                attemptAdd(parent);
            }
            attemptAdd(source.BaseType);
        }
        return foundParents;
    }

    /// <summary>Finds all possible assemblies.</summary>
    /// <returns>The found assemblies.</returns>
    private static HashSet<Assembly> FindAllAssemblies()
    {
        HashSet<Assembly> foundAssemblies = [.. AppDomain.CurrentDomain.GetAssemblies()];

        Stack<Assembly> sourceAssemblies = new(foundAssemblies);
        while (sourceAssemblies.Count > 0)
        {
            Assembly assembly = sourceAssemblies.Pop();
            foreach (AssemblyName referenced in assembly.GetReferencedAssemblies())
            {
                try
                {
                    Assembly loaded = Assembly.Load(referenced);
                    if (foundAssemblies.Add(loaded))
                    {
                        sourceAssemblies.Push(loaded);
                    }
                }
                catch
                {
                    // Ignore assemblies that can't be loaded.
                }
            }
        }
        return foundAssemblies;
    }

    /// <summary>Finds all instance fields on the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find fields on.</param>
    /// <returns>All found fields on the <see cref="Type"/>.</returns>
    /// <remarks>
    ///     The <see langword="private"/> fields in inherited <see cref="Type"/>s are included.
    /// </remarks>
    private static IEnumerable<FieldInfo> FindAllFields(Type? type)
    {
        if (type == null)
        {
            yield break;
        }

        foreach (
            FieldInfo field in type.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            )
        )
        {
            yield return field;
        }

        Type? currentType = type.BaseType;
        HashSet<Type> completedTypes = [type];
        while (currentType != null && completedTypes.Add(currentType))
        {
            foreach (
                FieldInfo field in currentType
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(f => f.IsPrivate)
            )
            {
                yield return field;
            }
            currentType = currentType.BaseType;
        }
    }

    /// <summary>Finds all instance properties on the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find properties on.</param>
    /// <returns>All found properties on the <see cref="Type"/>.</returns>
    /// <remarks>
    ///     The <see langword="private"/> properties in inherited <see cref="Type"/>s are included.
    /// </remarks>
    private static IEnumerable<PropertyInfo> FindAllProperties(Type? type)
    {
        if (type == null)
        {
            yield break;
        }

        foreach (
            PropertyInfo prop in type.GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            )
        )
        {
            yield return prop;
        }

        Type? currentType = type;
        HashSet<Type> completedTypes = [];
        while (currentType != null && completedTypes.Add(currentType))
        {
            foreach (
                PropertyInfo prop in currentType
                    .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(p =>
                        p.GetSetMethod(true)?.IsPrivate != false
                        && p.GetGetMethod(true)?.IsPrivate != false
                    )
            )
            {
                yield return prop;
            }
            currentType = currentType.BaseType;
        }
    }

    /// <summary>Finds all constructors on the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find constructors on.</param>
    /// <returns>All found constructors on the <see cref="Type"/>.</returns>
    private static ConstructorInfo[] FindAllConstructors(Type? type)
    {
        return type?.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            ) ?? [];
    }

    /// <summary>Finds all static methods that create the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find factories on.</param>
    /// <returns>All found factory methods on the <see cref="Type"/>.</returns>
    private static IEnumerable<MethodInfo> FindAllFactories(Type? type)
    {
        return type?.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.ReturnType.Inherits(type))
            ?? [];
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{nameof(InheritanceTracker)}({TypeDescriber.ExpandedName(SupportedType)})";
    }
}
