using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.Design;

namespace CreateAndFake;

/// <summary>Extensions for types.</summary>
public static class TypeExtensions
{
    /// <summary>Keeps track of available classes.</summary>
    private static readonly Dictionary<Assembly, ImmutableArray<Type>> _ClassTypeCache = [];

    /// <summary>Keeps track of type inheritance.</summary>
    private static readonly Dictionary<Type, FrozenSet<Type>> _InheritCache = [];

    /// <summary>Finds the defined interface with generics specified on <paramref name="type"/>.</summary>
    /// <param name="type">Type with the interface definition.</param>
    /// <param name="baseGenericType">Generic type definition to find.</param>
    /// <returns>The found interface.</returns>
    public static Type FindConcreteInterface(this Type type, Type baseGenericType)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        return type.GetInterfaces()
            .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == baseGenericType);
    }

    /// <summary>
    ///     Finds subclasses of <paramref name="type"/> in the <paramref name="type"/>'s assembly.
    /// </summary>
    /// <param name="type"><see cref="Type"/> to locate subclasses for.</param>
    /// <returns>The found creatable subclasses for <paramref name="type"/>.</returns>
    public static IEnumerable<Type> FindLocalSubclasses(this Type type)
    {
        ArgumentGuard.ThrowIfNull(type, nameof(type));

        return FindLoadedClassTypes(type.Assembly)
            .Where(t => !t.IsAbstract)
            .Where(t => t.Inherits(type))
            .Where(t => IsVisibleTo(t, Assembly.GetCallingAssembly().GetName()));
    }

    /// <summary>Finds subclasses of <paramref name="type"/> in all loaded assemblies.</summary>
    /// <param name="type"><see cref="Type"/> to locate subclasses for.</param>
    /// <returns>The found creatable subclasses for <paramref name="type"/>.</returns>
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

    /// <summary>Finds all types in <paramref name="assembly"/>.</summary>
    /// <param name="assembly"><see cref="Assembly"/> to load types from.</param>
    /// <returns>The found types if <paramref name="assembly"/> can load; none otherwise.</returns>
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
                _ClassTypeCache[assembly] = classTypes = [
                    .. FindLoadedTypes(assembly)
                        .Where(t => t.IsClass)
                        .Where(t => !t.IsNestedPrivate)
                        .Where(t => !t.IsDefined(typeof(CompilerGeneratedAttribute), false)),
                ];
            }
        }
        return classTypes;
    }

    /// <summary>Finds all types in <paramref name="assembly"/>.</summary>
    /// <param name="assembly"><see cref="Assembly"/> to load types from.</param>
    /// <returns>The found types if <paramref name="assembly"/> can load; none otherwise.</returns>
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

    /// <summary>Determines if <paramref name="type"/> is visible to <paramref name="assembly"/>.</summary>
    /// <param name="type"><see cref="Type"/> to check.</param>
    /// <param name="assembly">Name of the <see cref="Assembly"/>.</param>
    /// <returns>
    ///     <c>true</c> if <paramref name="type"/> is visible to
    ///     <paramref name="assembly"/>; <c>false</c> otherwise.
    /// </returns>
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

    /// <summary>Attempts to get the root generic type of <paramref name="type"/>.</summary>
    /// <param name="type"><see cref="Type"/> to cast.</param>
    /// <returns>The casted <paramref name="type"/> if generic; null otherwise.</returns>
    public static Type? AsGenericType(this Type? type)
    {
        return type != null && type.IsGenericType ? type.GetGenericTypeDefinition() : null;
    }

    /// <summary>Checks if <paramref name="parent"/> inherits <typeparamref name="T"/>.</summary>
    /// <typeparam name="T">Potential child <see cref="Type"/> of <paramref name="parent"/>.</typeparam>
    /// <param name="parent">Potential parent <see cref="Type"/> of <typeparamref name="T"/>.</param>
    /// <returns>
    ///     <c>true</c> if <paramref name="parent"/> inherits <typeparamref name="T"/>; <c>false</c> otherwise.
    /// </returns>
    public static bool Inherits<T>([NotNullWhen(true)] this Type? parent)
    {
        return Inherits(parent, typeof(T));
    }

    /// <summary>Checks if <paramref name="parent"/> inherits <paramref name="child"/>.</summary>
    /// <param name="parent">Potential parent <see cref="Type"/> of <paramref name="child"/>.</param>
    /// <param name="child">Potential child <see cref="Type"/> of <paramref name="parent"/>.</param>
    /// <returns>
    ///     <c>true</c> if <paramref name="parent"/> inherits <paramref name="child"/>; <c>false</c> otherwise.
    /// </returns>
    public static bool Inherits(
        [NotNullWhen(true)] this Type? parent,
        [NotNullWhen(true)] Type? child
    )
    {
        return IsInheritedBy(child, parent);
    }

    /// <summary>Checks if <typeparamref name="T"/> inherits <paramref name="child"/>.</summary>
    /// <typeparam name="T">Potential parent <see cref="Type"/> of <paramref name="child"/>.</typeparam>
    /// <param name="child">Potential child <see cref="Type"/> of <typeparamref name="T"/>.</param>
    /// <returns>
    ///     <c>true</c> if <typeparamref name="T"/> inherits <paramref name="child"/>; <c>false</c> otherwise.
    /// </returns>
    public static bool IsInheritedBy<T>([NotNullWhen(true)] this Type? child)
    {
        return IsInheritedBy(child, typeof(T));
    }

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

    /// <summary>Finds all types <paramref name="type"/> inherits.</summary>
    /// <param name="type"><see cref="Type"/> to check.</param>
    /// <returns>The found types inherited by <paramref name="type"/>.</returns>
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
