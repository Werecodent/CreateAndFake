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

    /// <summary>Finds <see langword="public"/> <typeparamref name="T"/> instance fields.</summary>
    /// <inheritdoc cref="GetPublicFields(Type?)"/>
    /// <inheritdoc cref="GetAllFields{T}"/>
    public static IEnumerable<FieldInfo> GetPublicFields<T>()
    {
        return GetPublicFields(typeof(T));
    }

    /// <summary>
    ///     Finds <see langword="public"/> instance fields on the <paramref name="type"/>.
    /// </summary>
    /// <remarks>Includes inherited <see langword="public"/> fields.</remarks>
    /// <inheritdoc cref="GetVisibleFields(Type?,AssemblyName)"/>
    public static IEnumerable<FieldInfo> GetPublicFields(Type? type)
    {
        return type?.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy
            ) ?? [];
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     <typeparamref name="T"/> instance fields.
    /// </summary>
    /// <inheritdoc cref="GetVisibleFields(Type?)"/>
    /// <inheritdoc cref="GetAllFields{T}"/>
    public static IEnumerable<FieldInfo> GetVisibleFields<T>()
    {
        return GetVisibleFields(typeof(T), Assembly.GetCallingAssembly().GetName());
    }

    /// <inheritdoc cref="GetVisibleFields(Type?,AssemblyName)"/>
    public static IEnumerable<FieldInfo> GetVisibleFields(Type? type)
    {
        return GetVisibleFields(type, Assembly.GetCallingAssembly().GetName());
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     instance fields on the <paramref name="type"/>.
    /// </summary>
    /// <param name="assembly">
    ///     Name of the <see cref="Assembly"/> to determine visibility for.
    /// </param>
    /// <remarks>
    ///     Finds <see langword="internal"/> fields only if they are visible to the calling method's
    ///     assembly. Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to access its <see langword="internal"/> fields for the test framework.
    /// </remarks>
    /// <inheritdoc cref="GetAllFields(Type?)"/>
    private static IEnumerable<FieldInfo> GetVisibleFields(Type? type, AssemblyName assembly)
    {
        if (InternalsAreVisible(type, assembly))
        {
            return GetAllFields(type).Where(f => f.IsPublic || f.IsAssembly);
        }
        else
        {
            return GetPublicFields(type);
        }
    }

    /// <summary>Finds all <typeparamref name="T"/> instance fields.</summary>
    /// <typeparam name="T">The <see cref="Type"/> to find fields on.</typeparam>
    /// <inheritdoc cref="GetAllFields(Type)"/>
    public static IEnumerable<FieldInfo> GetAllFields<T>()
    {
        return GetAllFields(typeof(T));
    }

    /// <summary>Finds all instance fields on the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find fields on.</param>
    /// <returns>All found fields on the <see cref="Type"/>.</returns>
    /// <remarks>
    ///     The <see langword="private"/> fields in inherited <see cref="Type"/>s are included.
    /// </remarks>
    public static IEnumerable<FieldInfo> GetAllFields(Type? type)
    {
        if (type == null)
        {
            yield break;
        }

        foreach (FieldInfo field in GetPublicFields(type))
        {
            yield return field;
        }

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

    /// <summary>
    ///     Finds <see langword="public"/> <typeparamref name="T"/> instance properties.
    /// </summary>
    /// <inheritdoc cref="GetPublicProperties(Type?)"/>
    /// <inheritdoc cref="GetAllProperties{T}"/>
    public static IEnumerable<PropertyInfo> GetPublicProperties<T>()
    {
        return GetPublicProperties(typeof(T));
    }

    /// <summary>
    ///     Finds <see langword="public"/> instance properties on the <paramref name="type"/>.
    /// </summary>
    /// <remarks>Includes inherited <see langword="public"/> properties.</remarks>
    /// <inheritdoc cref="GetVisibleProperties(Type?,AssemblyName)"/>
    public static IEnumerable<PropertyInfo> GetPublicProperties(Type? type)
    {
        return type?.GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy
            ) ?? [];
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     <typeparamref name="T"/> instance properties.
    /// </summary>
    /// <inheritdoc cref="GetVisibleProperties(Type?)"/>
    /// <inheritdoc cref="GetAllProperties{T}"/>
    public static IEnumerable<PropertyInfo> GetVisibleProperties<T>()
    {
        return GetVisibleProperties(typeof(T), Assembly.GetCallingAssembly().GetName());
    }

    /// <inheritdoc cref="GetVisibleProperties(Type?,AssemblyName)"/>
    public static IEnumerable<PropertyInfo> GetVisibleProperties(Type? type)
    {
        return GetVisibleProperties(type, Assembly.GetCallingAssembly().GetName());
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     instance properties on the <paramref name="type"/>.
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
    /// <inheritdoc cref="GetAllProperties(Type?)"/>
    public static IEnumerable<PropertyInfo> GetVisibleProperties(Type? type, AssemblyName assembly)
    {
        if (InternalsAreVisible(type, assembly))
        {
            return GetAllProperties(type)
                .Where(p =>
                {
                    MethodInfo? getMethod = p.GetGetMethod();
                    MethodInfo? setMethod = p.GetSetMethod();
                    return (getMethod != null && (getMethod.IsPublic || getMethod.IsAssembly))
                        || (setMethod != null && (setMethod.IsPublic || setMethod.IsAssembly));
                });
        }
        else
        {
            return GetPublicProperties(type);
        }
    }

    /// <summary>Finds all <typeparamref name="T"/> instance properties.</summary>
    /// <typeparam name="T">The <see cref="Type"/> to find properties on.</typeparam>
    /// <inheritdoc cref="GetAllProperties(Type)"/>
    public static IEnumerable<PropertyInfo> GetAllProperties<T>()
    {
        return GetAllProperties(typeof(T));
    }

    /// <summary>Finds all instance properties on the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find properties on.</param>
    /// <returns>All found properties on the <see cref="Type"/>.</returns>
    /// <remarks>
    ///     The <see langword="private"/> properties in inherited <see cref="Type"/>s are included.
    /// </remarks>
    public static IEnumerable<PropertyInfo> GetAllProperties(Type? type)
    {
        if (type == null)
        {
            yield break;
        }

        foreach (PropertyInfo prop in GetPublicProperties(type))
        {
            yield return prop;
        }

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

    /// <summary>Finds <see langword="public"/> <typeparamref name="T"/> constructors.</summary>
    /// <inheritdoc cref="GetPublicConstructors(Type?)"/>
    /// <inheritdoc cref="GetAllConstructors{T}"/>
    public static IEnumerable<ConstructorInfo> GetPublicConstructors<T>()
    {
        return GetPublicConstructors(typeof(T));
    }

    /// <summary>
    ///     Finds <see langword="public"/> constructors on the <paramref name="type"/>.
    /// </summary>
    /// <remarks>Includes inherited <see langword="public"/> constructors.</remarks>
    /// <inheritdoc cref="GetVisibleConstructors(Type?,AssemblyName)"/>
    public static IEnumerable<ConstructorInfo> GetPublicConstructors(Type? type)
    {
        return type?.GetConstructors(BindingFlags.Instance | BindingFlags.Public) ?? [];
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     <typeparamref name="T"/> constructors.
    /// </summary>
    /// <inheritdoc cref="GetVisibleConstructors(Type?)"/>
    /// <inheritdoc cref="GetAllConstructors{T}"/>
    public static IEnumerable<ConstructorInfo> GetVisibleConstructors<T>()
    {
        return GetVisibleConstructors(typeof(T), Assembly.GetCallingAssembly().GetName());
    }

    /// <inheritdoc cref="GetVisibleConstructors(Type?,AssemblyName)"/>
    public static IEnumerable<ConstructorInfo> GetVisibleConstructors(Type? type)
    {
        return GetVisibleConstructors(type, Assembly.GetCallingAssembly().GetName());
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     constructors on the <paramref name="type"/>.
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
    /// <inheritdoc cref="GetAllConstructors(Type?)"/>
    public static IEnumerable<ConstructorInfo> GetVisibleConstructors(
        Type? type,
        AssemblyName assembly
    )
    {
        if (InternalsAreVisible(type, assembly))
        {
            return GetAllConstructors(type).Where(c => c.IsPublic || c.IsAssembly);
        }
        else
        {
            return GetPublicConstructors(type);
        }
    }

    /// <summary>Finds all <typeparamref name="T"/> constructors.</summary>
    /// <typeparam name="T">The <see cref="Type"/> to find constructors on.</typeparam>
    /// <inheritdoc cref="GetAllConstructors(Type)"/>
    public static IEnumerable<ConstructorInfo> GetAllConstructors<T>()
    {
        return GetAllConstructors(typeof(T));
    }

    /// <summary>Finds all constructors on the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find constructors on.</param>
    /// <returns>All found constructors on the <see cref="Type"/>.</returns>
    public static IEnumerable<ConstructorInfo> GetAllConstructors(Type? type)
    {
        return type?.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            ) ?? [];
    }

    /// <summary>
    ///     Finds <see langword="public"/> static methods that create <typeparamref name="T"/>.
    /// </summary>
    /// <inheritdoc cref="GetPublicFactories(Type?)"/>
    /// <inheritdoc cref="GetAllFactories{T}"/>
    public static IEnumerable<MethodInfo> GetPublicFactories<T>()
    {
        return GetPublicFactories(typeof(T));
    }

    /// <summary>
    ///     Finds <see langword="public"/> static methods that create the <paramref name="type"/>.
    /// </summary>
    /// <inheritdoc cref="GetAllFactories(Type?)"/>
    public static IEnumerable<MethodInfo> GetPublicFactories(Type? type)
    {
        return type?.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(m => m.ReturnType.Inherits(type))
            ?? [];
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     static methods that create <typeparamref name="T"/>.
    /// </summary>
    /// <inheritdoc cref="GetVisibleFactories(Type?)"/>
    /// <inheritdoc cref="GetAllFactories{T}"/>
    public static IEnumerable<MethodInfo> GetVisibleFactories<T>()
    {
        return GetVisibleFactories(typeof(T), Assembly.GetCallingAssembly().GetName());
    }

    /// <inheritdoc cref="GetVisibleFactories(Type?,AssemblyName)"/>
    public static IEnumerable<MethodInfo> GetVisibleFactories(Type? type)
    {
        return GetVisibleFactories(type, Assembly.GetCallingAssembly().GetName());
    }

    /// <summary>
    ///     Finds <see langword="public"/> and <see langword="internal"/>
    ///     static methods that create the <paramref name="type"/>.
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
    /// <inheritdoc cref="GetAllFactories(Type?)"/>
    public static IEnumerable<MethodInfo> GetVisibleFactories(Type? type, AssemblyName assembly)
    {
        if (InternalsAreVisible(type, assembly))
        {
            return GetAllFactories(type).Where(c => !c.IsPrivate);
        }
        else
        {
            return GetPublicFactories(type);
        }
    }

    /// <summary>Finds all static methods that create <typeparamref name="T"/>.</summary>
    /// <typeparam name="T">The <see cref="Type"/> to find factories on.</typeparam>
    /// <inheritdoc cref="GetAllFactories(Type)"/>
    public static IEnumerable<MethodInfo> GetAllFactories<T>()
    {
        return GetAllFactories(typeof(T));
    }

    /// <summary>Finds all static methods that create the <paramref name="type"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to find factories on.</param>
    /// <returns>All found factory methods on the <see cref="Type"/>.</returns>
    public static IEnumerable<MethodInfo> GetAllFactories(Type? type)
    {
        return type?.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.ReturnType.Inherits(type))
            ?? [];
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
        return type != null && (type.IsVisible || InternalsAreVisible(type, assembly));
    }

    /// <summary>
    ///     Determines if the <paramref name="type"/>'s <see langword="internal"/>
    ///     members are usable in the <paramref name="assembly"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to verify visibility for.</param>
    /// <param name="assembly">
    ///     Name of the <see cref="Assembly"/> to check scope privilege for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="Type"/>'s <see langword="internal"/>s
    ///     are visible to the <paramref name="assembly"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to enable <see langword="internal"/> members visibility per this method.
    /// </remarks>
    private static bool InternalsAreVisible(Type? type, AssemblyName assembly)
    {
        return type?.Assembly.GetCustomAttributes<InternalsVisibleToAttribute>()
                .Any(a => a.AssemblyName == assembly.Name) == true;
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
