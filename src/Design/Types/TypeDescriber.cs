using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.Design.Extensions;
using Microsoft.CodeAnalysis;

namespace CreateAndFake.Design.Types;

/// <summary>Provides common <see cref="Type"/> patterns for getting additional details.</summary>
public static class TypeDescriber
{
    /// <summary>Prevents concurrency issues for <see cref="_ClassTypeCache"/>.</summary>
    private static readonly Lock _Lock = new();

    /// <summary>Caches every available <see cref="Type"/> per <see cref="Assembly"/>.</summary>
    private static readonly Dictionary<Assembly, ImmutableArray<Type>> _ClassTypeCache = [];

    /// <returns>The found inherited <see cref="Type"/>.</returns>
    /// <remarks>Example: <example><c>
    ///     FindConcreteInterface&lt;List&lt;int&gt;&gt;(typeof(IList&lt;&gt;))
    ///     == typeof(IList&lt;int&gt;) // true
    /// </c></example></remarks>
    /// <inheritdoc cref="AsConcreteType(Type)"/>
    /// <inheritdoc cref="FindConcreteType(Type,Type)"/>
    public static Type FindConcreteType<T>(Type genericBase)
    {
        return FindConcreteType(typeof(T), genericBase);
    }

    /// <returns>The found inherited <see cref="Type"/>.</returns>
    /// <exception cref="InvalidOperationException">
    ///     If the <see cref="Type"/> does not inherit <paramref name="genericBase"/>.
    /// </exception>
    /// <remarks>Example: <example><c>
    ///     FindConcreteInterface(typeof(List&lt;int&gt;), typeof(IList&lt;&gt;))
    ///     == typeof(IList&lt;int&gt;) // true
    /// </c></example></remarks>
    /// <inheritdoc cref="AsConcreteType(Type,Type)"/>
    public static Type FindConcreteType(Type child, Type genericBase)
    {
        return AsConcreteType(child, genericBase)
            ?? throw new InvalidOperationException(
                $"Type {child} doesn't inherit {genericBase} as a generic base class."
            );
    }

    /// <summary>
    ///     Finds the defined <paramref name="genericBase"/> with generics
    ///     specified that is inherited by <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> to find the generic base of.</typeparam>
    /// <remarks>Example: <example><c>
    ///     AsConcreteInterface&lt;List&lt;int&gt;&gt;(typeof(IList&lt;&gt;))
    ///     == typeof(IList&lt;int&gt;) // true
    /// </c></example></remarks>
    /// <inheritdoc cref="AsConcreteType(Type,Type)"/>
    public static Type? AsConcreteType<T>(Type genericBase)
    {
        return AsConcreteType(typeof(T), genericBase);
    }

    /// <summary>
    ///     Finds the defined <paramref name="genericBase"/> with generics specified
    ///     that is inherited by the <paramref name="child"/> <see cref="Type"/>.
    /// </summary>
    /// <param name="child">The <see cref="Type"/> to find the generic base of.</param>
    /// <param name="genericBase">
    ///     Generic <see cref="Type"/> definition without generics specified.
    /// </param>
    /// <returns>The inherited <see cref="Type"/> if found, null otherwise.</returns>
    /// <remarks>Example: <example><c>
    ///     AsConcreteInterface(typeof(List&lt;int&gt;), typeof(IList&lt;&gt;))
    ///     == typeof(IList&lt;int&gt;) // true
    /// </c></example></remarks>
    public static Type? AsConcreteType(Type? child, Type genericBase)
    {
        List<Type> inheritance = child?.GetInterfaces().ToList() ?? [];

        Type? current = child;
        while (current != null)
        {
            inheritance.Add(current);
            current = current.BaseType;
        }

        return inheritance
            .Where(i => i.IsGenericType)
            .Where(i => !i.IsGenericTypeDefinition)
            .SingleOrDefault(i => i.GetGenericTypeDefinition() == genericBase);
    }

    /// <summary>
    ///     Attempts to convert the <paramref name="type"/>
    ///     to its generic <see cref="Type"/> definition.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to convert.</param>
    /// <returns>
    ///     The generic <see cref="Type"/> definition for <paramref name="type"/> if it's generic,
    ///     <see langword="null"/> otherwise.
    /// </returns>
    public static Type? AsGenericBase(Type? type)
    {
        return type?.IsGenericType == true ? type.GetGenericTypeDefinition() : null;
    }

    /// <typeparam name="T">The <see cref="Type"/> to find fields on.</typeparam>
    /// <inheritdoc cref="GetAllFields(Type,bool)"/>
    public static IEnumerable<FieldInfo> GetAllFields<T>(bool onlyPublic = false)
    {
        return GetAllFields(typeof(T), onlyPublic);
    }

    /// <summary>Finds all instance fields on the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find fields on.</param>
    /// <param name="onlyPublic">If only public fields are to be returned.</param>
    /// <returns>All found fields on the <see cref="Type"/>.</returns>
    /// <remarks>
    ///     The <see langword="private"/> fields in inherited
    ///     <see cref="Type"/>s included by default.
    /// </remarks>
    public static IEnumerable<FieldInfo> GetAllFields(Type? type, bool onlyPublic = false)
    {
        if (type == null)
        {
            yield break;
        }

        foreach (
            FieldInfo field in type.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy
            )
        )
        {
            yield return field;
        }

        if (!onlyPublic)
        {
            Type? currentType = type;
            HashSet<Type> completedTypes = [];
            while (currentType != null && completedTypes.Add(currentType))
            {
                foreach (
                    FieldInfo field in currentType.GetFields(
                        BindingFlags.Instance | BindingFlags.NonPublic
                    )
                )
                {
                    yield return field;
                }
                currentType = currentType.BaseType;
            }
        }
    }

    /// <typeparam name="T"><see cref="Type"/> to find properties on.</typeparam>
    /// <inheritdoc cref="GetAllProperties(Type,bool)"/>
    public static IEnumerable<PropertyInfo> GetAllProperties<T>(bool onlyPublic = false)
    {
        return GetAllProperties(typeof(T), onlyPublic);
    }

    /// <summary>Finds all instance properties on the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find properties on.</param>
    /// <param name="onlyPublic">If only public properties are to be returned.</param>
    /// <returns>All found properties on the <see cref="Type"/>.</returns>
    /// <remarks>
    ///     The <see langword="private"/> properties in
    ///     inherited <see cref="Type"/>s included by default.
    /// </remarks>
    public static IEnumerable<PropertyInfo> GetAllProperties(Type? type, bool onlyPublic = false)
    {
        if (type == null)
        {
            yield break;
        }

        foreach (
            PropertyInfo prop in type.GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy
            )
        )
        {
            yield return prop;
        }

        if (!onlyPublic)
        {
            Type? currentType = type;
            HashSet<Type> completedTypes = [];
            while (currentType != null && completedTypes.Add(currentType))
            {
                foreach (
                    PropertyInfo prop in currentType.GetProperties(
                        BindingFlags.Instance | BindingFlags.NonPublic
                    )
                )
                {
                    if (prop.CanRead)
                    {
                        yield return prop;
                    }
                }
                currentType = currentType.BaseType;
            }
        }
    }

    /// <summary>
    ///     Finds every non-<see langword="abstract"/> <see langword="class"/>
    ///     inheriting <typeparamref name="T"/> in its defined <see cref="Assembly"/>.
    /// </summary>
    /// <inheritdoc cref="FindLoadedSubclasses{T}"/>
    public static IEnumerable<Type> FindLocalSubclasses<T>()
    {
        return FindLocalSubclasses(typeof(T));
    }

    /// <summary>
    ///     Finds every non-<see langword="abstract"/> <see langword="class"/>
    ///     inheriting the <paramref name="type"/> in its defined <see cref="Assembly"/>.
    /// </summary>
    /// <inheritdoc cref="FindLoadedSubclasses"/>
    public static IEnumerable<Type> FindLocalSubclasses(Type? type)
    {
        if (type == null)
        {
            return [];
        }

        return FindLoadedClassTypes(type.Assembly)
            .Where(t => !t.IsAbstract)
            .Where(t => t.Inherits(type))
            .Where(t => IsVisible(t, Assembly.GetCallingAssembly().GetName()));
    }

    /// <summary>
    ///     Finds every non-<see langword="abstract"/> <see langword="class"/>
    ///     inheriting <typeparamref name="T"/> in all loaded assemblies.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> to find subclasses for.</typeparam>
    /// <inheritdoc cref="FindLoadedSubclasses(Type)"/>
    public static IEnumerable<Type> FindLoadedSubclasses<T>()
    {
        return FindLoadedSubclasses(typeof(T));
    }

    /// <summary>
    ///     Finds every non-<see langword="abstract"/> <see langword="class"/>
    ///     inheriting the <paramref name="type"/> in all loaded assemblies.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to find subclasses for.</param>
    /// <returns>The found creatable subclasses.</returns>
    /// <remarks>
    ///     Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to access its <see langword="internal"/> types for this method.
    /// </remarks>
    public static IEnumerable<Type> FindLoadedSubclasses(Type? type)
    {
        return AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(a => !a.ReflectionOnly)
            .Where(a => !a.IsDynamic)
            .SelectMany(FindLoadedClassTypes)
            .Where(t => !t.IsAbstract)
            .Where(t => t.Inherits(type))
            .Where(t => IsVisible(t, Assembly.GetCallingAssembly().GetName()));
    }

    /// <summary>Finds every <see langword="class"/> in the <paramref name="assembly"/>.</summary>
    /// <param name="assembly">
    ///     <see cref="Assembly"/> containing the <see langword="class"/>es to search for.
    /// </param>
    /// <returns>
    ///     Every found <see langword="class"/> if the
    ///     <paramref name="assembly"/> loads, none otherwise.
    /// </returns>
    public static IEnumerable<Type> FindLoadedClassTypes(Assembly? assembly)
    {
        if (assembly == null)
        {
            return [];
        }

        ImmutableArray<Type> classTypes;
        lock (_Lock)
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

    /// <summary>Finds every <see cref="Type"/> in the <paramref name="assembly"/>.</summary>
    /// <param name="assembly">
    ///     <see cref="Assembly"/> containing the <see cref="Type"/>s to search for.
    /// </param>
    /// <returns>
    ///     Every found <see cref="Type"/> if the
    ///     <paramref name="assembly"/> can load, none otherwise.
    /// </returns>
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
    ///     Determines if <typeparamref name="T"/> is usable in the <paramref name="assembly"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> to verify visibility for.</typeparam>
    /// <inheritdoc cref="IsVisible(Type,AssemblyName)"/>
    public static bool IsVisible<T>(AssemblyName assembly)
    {
        return IsVisible(typeof(T), assembly);
    }

    /// <summary>
    ///     Determines if the <paramref name="type"/> is usable in the <paramref name="assembly"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to verify visibility for.</param>
    /// <param name="assembly">
    ///     Name of the <see cref="Assembly"/> trying to use the <see cref="Type"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="Type"/> is visible to
    ///     the <paramref name="assembly"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to access its <see langword="internal"/> types for this method.
    /// </remarks>
    public static bool IsVisible(Type? type, AssemblyName assembly)
    {
        return type != null
            && (
                type.IsVisible
                || type.Assembly.GetCustomAttributes<InternalsVisibleToAttribute>()
                    .Any(a => a.AssemblyName == assembly.Name)
            );
    }

    /// <summary>Builds a <see cref="Type"/> name with any generics included.</summary>
    /// <param name="instance">The instance to create a <see cref="Type"/> name for.</param>
    /// <returns>The built display name.</returns>
    public static string ExpandedName(object? instance)
    {
        return ExpandedName(instance is Type type ? type : instance?.GetType());
    }

    /// <summary>Builds a <typeparamref name="T"/> name with any generics included.</summary>
    /// <typeparam name="T">The <see cref="Type"/> to create a name for.</typeparam>
    /// <returns>The built display name.</returns>
    public static string ExpandedName<T>()
    {
        return ExpandedName(typeof(T));
    }

    /// <summary>Builds a <paramref name="type"/> name with any generics included.</summary>
    /// <param name="type">The <see cref="Type"/> to create a name for.</param>
    /// <returns>The built display name.</returns>
    public static string ExpandedName(Type? type)
    {
        if (type?.IsGenericType == true)
        {
            return string.Concat(
                type.Name.Substring(0, type.Name.IndexOf("`", StringComparison.InvariantCulture)),
                "<",
                string.Join(",", type.GetGenericArguments().Select(ExpandedName)),
                ">"
            );
        }
        else
        {
            return type?.Name ?? "";
        }
    }

    /// <summary>Builds a display name for the <paramref name="method"/> under test.</summary>
    /// <param name="method">The method being tested needing a name.</param>
    /// <returns>The built display name.</returns>
    public static string BuildTestName(MethodBase method)
    {
        if (method != null)
        {
            IEnumerable<string> paramNames = method
                .GetParameters()
                .Select(p => ExpandedName(p.ParameterType));

            return $"{method.Name}({string.Join(",", paramNames)})";
        }
        else
        {
            return "";
        }
    }
}
