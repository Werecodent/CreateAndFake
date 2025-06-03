using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.Design;

namespace CreateAndFake;

/// <summary>Inheritance methods to extend the <see cref="Type"/> <see langword="class"/>.</summary>
public static class TypeExtensions
{
    /// <summary>Caches every available <see cref="Type"/> per <see cref="Assembly"/>.</summary>
    private static readonly Dictionary<Assembly, ImmutableArray<Type>> _ClassTypeCache = [];

    /// <summary>
    ///     Caches every child <see cref="Type"/> inherited per parent <see cref="Type"/>.
    /// </summary>
    private static readonly Dictionary<Type, FrozenSet<Type>> _InheritCache = [];

    /// <summary>
    ///     Finds the defined <see langword="interface"/> with generics
    ///     specified on <paramref name="type"/> (<see langword="this"/>).
    /// </summary>
    /// <param name="type">
    ///     <see cref="Type"/> with the <see langword="interface"/> definition.
    /// </param>
    /// <param name="baseGenericType">
    ///     Generic <see cref="Type"/> definition without specified generics.
    /// </param>
    /// <returns>The found defined <see langword="interface"/>.</returns>
    public static Type FindConcreteInterface(this Type type, Type baseGenericType)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        return type.GetInterfaces()
            .Where(i => i.IsGenericType)
            .Single(i => i.GetGenericTypeDefinition() == baseGenericType);
    }

    /// <summary>
    ///     Finds subclasses of <paramref name="type"/>
    ///     (<see langword="this"/>) from its defined <see cref="Assembly"/>.
    /// </summary>
    /// <param name="type"><see cref="Type"/> to locate subclasses for.</param>
    /// <returns>The found creatable subclasses for <paramref name="type"/>.</returns>
    /// <remarks>
    ///     Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to access its <see langword="internal"/> types for this method.
    /// </remarks>
    public static IEnumerable<Type> FindLocalSubclasses(this Type type)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        return FindLoadedClassTypes(type.Assembly)
            .Where(t => !t.IsAbstract)
            .Where(t => t.Inherits(type))
            .Where(t => IsVisibleTo(t, Assembly.GetCallingAssembly().GetName()));
    }

    /// <summary>
    ///     Finds subclasses of <paramref name="type"/>
    ///     (<see langword="this"/>) in every loaded <see cref="Assembly"/>.
    /// </summary>
    /// <param name="type"><see cref="Type"/> to locate subclasses for.</param>
    /// <returns>The found creatable subclasses for <paramref name="type"/>.</returns>
    /// <remarks>
    ///     Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to access its <see langword="internal"/> types for this method.
    /// </remarks>
    public static IEnumerable<Type> FindLoadedSubclasses(this Type type)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        return AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(a => !a.ReflectionOnly)
            .Where(a => !a.IsDynamic)
            .SelectMany(FindLoadedClassTypes)
            .Where(t => !t.IsAbstract)
            .Where(t => t.Inherits(type))
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
    public static bool IsVisibleTo([NotNullWhen(true)] this Type? type, AssemblyName assembly)
    {
        ArgumentGuard.ThrowIfNull(assembly, nameof(assembly));

        return type != null
            && (
                type.IsVisible
                || type.Assembly.GetCustomAttributes<InternalsVisibleToAttribute>()
                    .Any(a => a.AssemblyName == assembly.Name)
            );
    }

    /// <summary>
    ///     Attempts to cast <paramref name="type"/> (<see langword="this"/>)
    ///     to its generic <see cref="Type"/> definition.
    /// </summary>
    /// <param name="type"><see cref="Type"/> to cast.</param>
    /// <returns>
    ///     The casted <paramref name="type"/> if generic, <see langword="null"/> otherwise.
    /// </returns>
    public static Type? AsGenericType(this Type? type)
    {
        return type?.IsGenericType == true ? type.GetGenericTypeDefinition() : null;
    }

    /// <summary>
    ///     Checks if <paramref name="parent"/>
    ///     (<see langword="this"/>) inherits <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     Potential child <see cref="Type"/> of <paramref name="parent"/>.
    /// </typeparam>
    /// <param name="parent">Potential parent of <typeparamref name="T"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="parent"/> inherits <typeparamref name="T"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool Inherits<T>([NotNullWhen(true)] this Type? parent)
    {
        return Inherits(parent, typeof(T));
    }

    /// <summary>
    ///     Checks if <paramref name="parent"/>
    ///     (<see langword="this"/>) inherits <paramref name="child"/>.
    /// </summary>
    /// <param name="parent">Potential parent of <paramref name="child"/>.</param>
    /// <param name="child">Potential child of <paramref name="parent"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="parent"/> inherits <paramref name="child"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool Inherits(
        [NotNullWhen(true)] this Type? parent,
        [NotNullWhen(true)] Type? child
    )
    {
        return IsInheritedBy(child, parent);
    }

    /// <summary>
    ///     Checks if <typeparamref name="T"/> inherits
    ///     <paramref name="child"/> (<see langword="this"/>).
    /// </summary>
    /// <typeparam name="T">
    ///     Potential parent <see cref="Type"/> of <paramref name="child"/>.
    /// </typeparam>
    /// <param name="child">Potential child of <typeparamref name="T"/>.</param>
    /// <returns>
    ///     <see langword="true"/> if <typeparamref name="T"/> inherits <paramref name="child"/>,
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsInheritedBy<T>([NotNullWhen(true)] this Type? child)
    {
        return IsInheritedBy(child, typeof(T));
    }

    /// <summary>
    ///     Checks if <paramref name="parent"/> inherits
    ///     <paramref name="child"/> (<see langword="this"/>).
    /// </summary>
    /// <inheritdoc cref="Inherits"/>
    public static bool IsInheritedBy(
        [NotNullWhen(true)] this Type? child,
        [NotNullWhen(true)] Type? parent
    )
    {
        if (child == null || parent == null)
        {
            return false;
        }

        FrozenSet<Type>? children;
        lock (_InheritCache)
        {
            if (!_InheritCache.TryGetValue(parent, out children))
            {
                _InheritCache[parent] = children = FindInheritance(parent).ToFrozenSet();
            }
        }

        return children.Contains(Nullable.GetUnderlyingType(child) ?? child);
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
