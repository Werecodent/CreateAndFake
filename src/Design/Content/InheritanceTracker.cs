using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Design.Content;

/// <summary>Finds all parents (base classes/interfaces) for <see cref="Type"/>s.</summary>
public sealed class InheritanceTracker : ITypeSupporter
{
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
                HashSet<Type> parents = [];
                FindInheritance(type, parents);
                describer = _InheritCache[type] = new(type, parents);
            }
        }
        return describer;
    }

    /// <inheritdoc/>
    public Type? SupportedType { get; }

    /// <summary>All found inherited <see cref="Type"/>s.</summary>
    public IEnumerable<Type> InheritedTypes { get; }

    /// <summary><inheritdoc cref="InheritanceTracker"/></summary>
    /// <param name="type"><inheritdoc cref="SupportedType" path="/summary"/></param>
    /// <param name="parents"><inheritdoc cref="InheritedTypes" path="/summary"/></param>
    private InheritanceTracker(Type? type, IEnumerable<Type> parents)
    {
        SupportedType = type;
        InheritedTypes = parents.ToFrozenSet();
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
    ///     Finds every <see cref="Type"/> that the <paramref name="type"/>
    ///     inherits and adds them to <paramref name="foundParents"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to find base classes/interfaces for.</param>
    /// <param name="foundParents">Collection to add all found base classes/interfaces to.</param>
    private static void FindInheritance(Type? type, ISet<Type> foundParents)
    {
        if (type != null && foundParents.Add(type))
        {
            if (type.IsGenericType)
            {
                FindInheritance(type.GetGenericTypeDefinition(), foundParents);
            }

            foreach (Type child in type.GetInterfaces())
            {
                FindInheritance(child, foundParents);
            }

            FindInheritance(type.BaseType, foundParents);
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{nameof(InheritanceTracker)}({TypeDescriber.ExpandedName(SupportedType)})";
    }
}
