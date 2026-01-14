using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CreateAndFake.Design.Content;

/// <summary>Finds details for types.</summary>
public sealed class InheritanceTracker
{
    /// <summary>
    ///     Caches every child <see cref="Type"/> inherited per parent <see cref="Type"/>.
    /// </summary>
    private static readonly Dictionary<Type, InheritanceTracker> _InheritCache = [];

    /// <summary>Tracker for null types.</summary>
    private static readonly InheritanceTracker _NullDescriber = new(null, []);

    /// <summary>Finds the describer for <typeparamref name="T"/>.</summary>
    /// <typeparam name="T">Type to describe.</typeparam>
    /// <returns>Found describer.</returns>
    public static InheritanceTracker For<T>()
    {
        return For(typeof(T));
    }

    /// <summary>Finds the describer for <paramref name="type"/>.</summary>
    /// <param name="type">Type to describe.</param>
    /// <returns>Found describer.</returns>
    public static InheritanceTracker For(Type? type)
    {
        if (type == null)
        {
            return _NullDescriber;
        }

        InheritanceTracker? describer;
        lock (_InheritCache)
        {
            if (!_InheritCache.TryGetValue(type, out describer))
            {
                HashSet<Type> children = [];
                FindInheritance(type, children);
                _InheritCache[type] = describer = new(type, children);
            }
        }
        return describer;
    }

    /// <summary>Type being described.</summary>
    private readonly Type? _type;

    /// <summary>All inherited types for the type.</summary>
    private readonly FrozenSet<Type> _children;

    /// <inheritdoc cref="_children"/>
    public IEnumerable<Type> InheritedTypes => _children;

    /// <summary><inheritdoc cref="TypeDescriber"/></summary>
    /// <param name="type"><inheritdoc cref="_type" path="/summary"/></param>
    /// <param name="children"><inheritdoc cref="_children" path="/summary"/></param>
    internal InheritanceTracker(Type? type, IEnumerable<Type> children)
    {
        _type = type;
        _children = children.ToFrozenSet();
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
    ///     Checks if this type (<see langword="this"/>) inherits <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"> Potential child <see cref="Type"/> of this type.</typeparam>
    /// <returns>
    ///     <see langword="true"/> if this type inherits <typeparamref name="T"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public bool Inherits<T>()
    {
        return Inherits(typeof(T));
    }

    /// <summary>
    ///     Checks if the type (<see langword="this"/>) inherits <paramref name="child"/>.
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
    /// <param name="foundChildren">Collection to find all children to.</param>
    /// <returns>Every found <see cref="Type"/> inherited by <paramref name="type"/>.</returns>
    private static void FindInheritance(Type? type, ISet<Type> foundChildren)
    {
        if (type != null && foundChildren.Add(type))
        {
            if (type.IsGenericType)
            {
                FindInheritance(type.GetGenericTypeDefinition(), foundChildren);
            }

            foreach (Type child in type.GetInterfaces())
            {
                FindInheritance(child, foundChildren);
            }

            FindInheritance(type.BaseType, foundChildren);
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{TypeDescriber.ExpandedName(GetType())}({TypeDescriber.ExpandedName(_type)})";
    }
}
