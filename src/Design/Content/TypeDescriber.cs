using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CreateAndFake.Design.Content;

/// <summary>Finds details for types.</summary>
public sealed class TypeDescriber
{
    /// <summary>Caches every available <see cref="Type"/> per <see cref="Assembly"/>.</summary>
    private static readonly Dictionary<Assembly, ImmutableArray<Type>> _ClassTypeCache = [];

    /// <summary>
    ///     Caches every child <see cref="Type"/> inherited per parent <see cref="Type"/>.
    /// </summary>
    private static readonly Dictionary<Type, TypeDescriber> _InheritCache = [];

    private static readonly TypeDescriber _NullDescriber = new(null, []);

    /// <summary>Finds the describer for <typeparamref name="T"/>.</summary>
    /// <typeparam name="T">Type to describe.</typeparam>
    /// <returns>Found describer.</returns>
    public static TypeDescriber For<T>()
    {
        return For(typeof(T));
    }

    /// <summary>Finds the describer for <paramref name="type"/>.</summary>
    /// <param name="type">Type to describe.</param>
    /// <returns>Found describer.</returns>
    public static TypeDescriber For(Type? type)
    {
        if (type == null)
        {
            return _NullDescriber;
        }

        TypeDescriber? describer;
        lock (_InheritCache)
        {
            if (!_InheritCache.TryGetValue(type, out describer))
            {
                _InheritCache[type] = describer = new(type, FindInheritance(type));
            }
        }
        return describer;
    }

    /// <summary>Type being described.</summary>
    private readonly Type? _type;

    /// <summary>All inherited types for the type.</summary>
    private readonly FrozenSet<Type> _children;

    /// <summary><inheritdoc cref="TypeDescriber"/></summary>
    /// <param name="type"><inheritdoc cref="_type" path="/summary"/></param>
    /// <param name="children"><inheritdoc cref="_children" path="/summary"/></param>
    internal TypeDescriber(Type? type, IEnumerable<Type> children)
    {
        _type = type;
        _children = children.ToFrozenSet();
    }

    /// <summary>
    ///     Finds the defined <see langword="interface"/> with generics
    ///     specified on the type (<see langword="this"/>).
    /// </summary>
    /// <param name="genericBase">
    ///     Generic <see cref="Type"/> definition without specified generics.
    /// </param>
    /// <returns>The found defined <see langword="interface"/>.</returns>
    /// <exception cref="InvalidOperationException">If null or missing.</exception>
    public Type FindConcreteInterface(Type genericBase)
    {
        return _type
                ?.GetInterfaces()
                .Where(i => i.IsGenericType)
                .SingleOrDefault(i => i.GetGenericTypeDefinition() == genericBase)
            ?? throw new InvalidOperationException($"Type {_type} doesn't inherit {genericBase}.");
    }

    /// <summary>Get all fields.</summary>
    /// <returns>All fields.</returns>
    public IEnumerable<FieldInfo> GetAllFields()
    {
        if (_type == null)
        {
            yield break;
        }

        foreach (
            FieldInfo field in _type.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            )
        )
        {
            yield return field;
        }

        Type? baseType = _type.BaseType;
        int i = 0;
        while (baseType != null && baseType != typeof(object) && i++ < 10)
        {
            foreach (
                FieldInfo field in baseType.GetFields(
                    BindingFlags.Instance | BindingFlags.NonPublic
                )
            )
            {
                if (field.IsPrivate)
                {
                    yield return field;
                }
            }
            baseType = baseType.BaseType;
        }
    }

    /// <summary>Get all properties.</summary>
    /// <returns>All properties.</returns>
    public IEnumerable<PropertyInfo> GetAllProperties()
    {
        if (_type == null)
        {
            yield break;
        }

        foreach (
            PropertyInfo prop in _type.GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            )
        )
        {
            yield return prop;
        }

        Type? baseType = _type.BaseType;
        int i = 0;
        while (baseType != null && baseType != typeof(object) && i++ < 10)
        {
            foreach (
                PropertyInfo prop in baseType.GetProperties(
                    BindingFlags.Instance | BindingFlags.NonPublic
                )
            )
            {
                if (prop.CanRead && (prop.GetGetMethod()?.IsPrivate ?? false))
                {
                    yield return prop;
                }
            }
            baseType = baseType.BaseType;
        }
    }

    /// <summary>
    ///     Finds subclasses of the type (<see langword="this"/>)
    ///     from its defined <see cref="Assembly"/>.
    /// </summary>
    /// <returns>The found creatable subclasses for the type.</returns>
    /// <remarks>
    ///     Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to access its <see langword="internal"/> types for this method.
    /// </remarks>
    public IEnumerable<Type> FindLocalSubclasses()
    {
        if (_type == null)
        {
            return [];
        }

        return FindLoadedClassTypes(_type.Assembly)
            .Where(t => !t.IsAbstract)
            .Where(t => t.Inherits(_type))
            .Where(t => IsVisibleTo(t, Assembly.GetCallingAssembly().GetName()));
    }

    /// <summary>
    ///     Finds subclasses of the type (<see langword="this"/>)
    ///     in every loaded <see cref="Assembly"/>.
    /// </summary>
    /// <returns>The found creatable subclasses for the type.</returns>
    /// <remarks>
    ///     Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to access its <see langword="internal"/> types for this method.
    /// </remarks>
    public IEnumerable<Type> FindLoadedSubclasses()
    {
        return AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(a => !a.ReflectionOnly)
            .Where(a => !a.IsDynamic)
            .SelectMany(FindLoadedClassTypes)
            .Where(t => !t.IsAbstract)
            .Where(t => t.Inherits(_type))
            .Where(t => IsVisibleTo(t, Assembly.GetCallingAssembly().GetName()));
    }

    /// <summary>
    ///     Find every <see langword="class"/> <see cref="Type"/> in <paramref name="assembly"/>.
    /// </summary>
    /// <param name="assembly"><see cref="Type"/> container to search.</param>
    /// <returns>The found types if <paramref name="assembly"/> can load, none otherwise.</returns>
    internal static IEnumerable<Type> FindLoadedClassTypes(Assembly? assembly)
    {
        if (assembly == null)
        {
            return [];
        }

        ImmutableArray<Type> classTypes;
        lock (_ClassTypeCache)
        {
            if (!_ClassTypeCache.TryGetValue(assembly, out classTypes))
            {
                IEnumerable<Type> types = FindLoadedTypes(assembly)
                    .Where(t => t.IsClass)
                    .Where(t => !t.IsNestedPrivate)
                    .Where(t => !t.IsDefined(typeof(CompilerGeneratedAttribute), false));

                _ClassTypeCache[assembly] = classTypes = [.. types];
            }
        }
        return classTypes;
    }

    /// <summary>Finds every <see cref="Type"/> in <paramref name="assembly"/>.</summary>
    /// <param name="assembly"><see cref="Type"/> container to search.</param>
    /// <returns>The found types if <paramref name="assembly"/> can load, none otherwise.</returns>
    internal static IEnumerable<Type> FindLoadedTypes(Assembly? assembly)
    {
        try
        {
            return assembly?.GetTypes() ?? Type.EmptyTypes;
        }
        catch (FileNotFoundException)
        {
            return Type.EmptyTypes;
        }
        catch (ReflectionTypeLoadException)
        {
            return Type.EmptyTypes;
        }
    }

    /// <summary>
    ///     Determines if the type can be used by <paramref name="assembly"/>.
    /// </summary>
    /// <param name="assembly">Name of the <see cref="Assembly"/> to verify access for.</param>
    /// <returns>
    ///     <see langword="true"/> if the type is visible to
    ///     <paramref name="assembly"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     Mark the <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c> to
    ///     return <see langword="true"/> for its <see langword="internal"/> types with this method.
    /// </remarks>
    public bool IsVisibleTo(AssemblyName assembly)
    {
        return IsVisibleTo(_type, assembly);
    }

    /// <summary>
    ///     Determines if <paramref name="type"/> (<see langword="this"/>)
    ///     can be used by <paramref name="assembly"/>.
    /// </summary>
    /// <param name="type"><see cref="Type"/> to check.</param>
    /// <param name="assembly">Name of the <see cref="Assembly"/> to verify access for.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="type"/> is visible to
    ///     <paramref name="assembly"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     Mark the <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c> to
    ///     return <see langword="true"/> for its <see langword="internal"/> types with this method.
    /// </remarks>
    public static bool IsVisibleTo(Type? type, AssemblyName assembly)
    {
        return type != null
            && (
                type.IsVisible
                || type.Assembly.GetCustomAttributes<InternalsVisibleToAttribute>()
                    .Any(a => a.AssemblyName == assembly.Name)
            );
    }

    /// <summary>
    ///     Checks if this type
    ///     (<see langword="this"/>) inherits <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     Potential child <see cref="Type"/> of this type.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true"/> if this type inherits <typeparamref name="T"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public bool Inherits<T>()
    {
        return Inherits(typeof(T));
    }

    /// <summary>
    ///     Checks if the type
    ///     (<see langword="this"/>) inherits <paramref name="child"/>.
    /// </summary>
    /// <param name="child">Potential child of the type.</param>
    /// <returns>
    ///     <see langword="true"/> if the type inherits <paramref name="child"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public bool Inherits([NotNullWhen(true)] Type? child)
    {
        if (child == null || _type == null)
        {
            return false;
        }
        return _children.Contains(Nullable.GetUnderlyingType(child) ?? child);
    }

    /// <summary>Finds every <see cref="Type"/> that <paramref name="type"/> inherits.</summary>
    /// <param name="type"><see cref="Type"/> to find children for.</param>
    /// <returns>Every found <see cref="Type"/> inherited by <paramref name="type"/>.</returns>
    private static IEnumerable<Type> FindInheritance(Type? type)
    {
        if (type == null)
        {
            yield break;
        }

        yield return type;

        if (type.IsGenericType)
        {
            yield return type.GetGenericTypeDefinition();
        }

        foreach (Type child in type.GetInterfaces().SelectMany(FindInheritance))
        {
            yield return child;
        }

        foreach (Type child in FindInheritance(type.BaseType))
        {
            yield return child;
        }
    }
}
