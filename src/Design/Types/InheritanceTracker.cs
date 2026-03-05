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

    /// <inheritdoc cref="SubTypes"/>
    private readonly Lazy<FrozenSet<Type>> _subTypes;

    /// <summary><inheritdoc cref="InheritanceTracker"/></summary>
    /// <param name="type"><inheritdoc cref="SupportedType" path="/summary"/></param>
    /// <param name="parents"><inheritdoc cref="InheritedTypes" path="/summary"/></param>
    private InheritanceTracker(Type? type, IEnumerable<Type> parents)
    {
        SupportedType = type;
        InheritedTypes = parents.ToFrozenSet();
        _subTypes = new(() => FindLoadedChildren(type).ToFrozenSet());
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

    /// <inheritdoc cref="FindLocalSubclasses(AssemblyName)"/>
    public IEnumerable<Type> FindLocalSubclasses()
    {
        return FindLocalSubclasses(Assembly.GetCallingAssembly().GetName());
    }

    /// <summary>
    ///     Finds every non-<see langword="abstract"/> <see langword="class"/> inheriting
    ///     the <see cref="SupportedType"/> in its defined <see cref="Assembly"/>.
    /// </summary>
    /// <inheritdoc cref="FindLoadedSubclasses(AssemblyName)"/>
    private IEnumerable<Type> FindLocalSubclasses(AssemblyName assembly)
    {
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

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{nameof(InheritanceTracker)}({TypeDescriber.ExpandedName(SupportedType)})";
    }
}
