using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace CreateAndFake.Design.Types;

/// <summary>Provides common <see cref="Type"/> patterns for getting additional details.</summary>
public static class TypeHelper
{
    /// <returns>The found inherited <see cref="Type"/>.</returns>
    /// <remarks>Example: <example><c>
    ///     FindConcreteInterface&lt;List&lt;int&gt;&gt;(typeof(IList&lt;&gt;)) == typeof(IList&lt;int&gt;) // true
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
    ///     FindConcreteInterface(typeof(List&lt;int&gt;), typeof(IList&lt;&gt;)) == typeof(IList&lt;int&gt;) // true
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
    ///     AsConcreteInterface&lt;List&lt;int&gt;&gt;(typeof(IList&lt;&gt;)) == typeof(IList&lt;int&gt;) // true
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
    /// <param name="genericBase">Generic <see cref="Type"/> definition without generics specified.</param>
    /// <returns>The inherited <see cref="Type"/> if found, null otherwise.</returns>
    /// <remarks>Example: <example><c>
    ///     AsConcreteInterface(typeof(List&lt;int&gt;), typeof(IList&lt;&gt;)) == typeof(IList&lt;int&gt;) // true
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

    /// <summary>Attempts to convert the <paramref name="type"/> to its generic <see cref="Type"/> definition.</summary>
    /// <param name="type">The <see cref="Type"/> to convert.</param>
    /// <returns>
    ///     The generic <see cref="Type"/> definition for <paramref name="type"/> if it's generic,
    ///     <see langword="null"/> otherwise.
    /// </returns>
    public static Type? AsGenericBase(Type? type)
    {
        return type?.IsGenericType == true ? type.GetGenericTypeDefinition() : null;
    }

    /// <summary>Determines if <typeparamref name="T"/> is usable in the <paramref name="assembly"/>.</summary>
    /// <typeparam name="T">The <see cref="Type"/> to verify visibility for.</typeparam>
    /// <inheritdoc cref="IsVisible(Type,AssemblyName)"/>
    public static bool IsVisible<T>(AssemblyName assembly)
    {
        return IsVisible(typeof(T), assembly);
    }

    /// <summary>Determines if the <paramref name="type"/> is usable in the <paramref name="assembly"/>.</summary>
    /// <param name="type">The <see cref="Type"/> to verify visibility for.</param>
    /// <param name="assembly">Name of the <see cref="Assembly"/> trying to use the <see cref="Type"/>.</param>
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
    /// <param name="assembly">Name of the <see cref="Assembly"/> to check scope privilege for.</param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="Type"/>'s <see langword="internal"/>s
    ///     are visible to the <paramref name="assembly"/>, <see langword="false"/> otherwise.
    /// </returns>
    /// <remarks>
    ///     Mark an <see cref="Assembly"/> with <c>InternalsVisibleTo("CreateAndFake")</c>
    ///     to enable <see langword="internal"/> members visibility per this method.
    /// </remarks>
    internal static bool InternalsAreVisible(Type? type, AssemblyName assembly)
    {
        ArgumentGuard.ThrowIfNull(assembly);

        Assembly? typeAssembly = type?.Assembly;
        return typeAssembly != null
            && (
                typeAssembly.FullName == assembly.FullName
                || typeAssembly
                    .GetCustomAttributes<InternalsVisibleToAttribute>()
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

    /// <summary>Finds every <see langword="class"/> in the <paramref name="assembly"/>.</summary>
    /// <param name="assembly"><see cref="Assembly"/> containing the <see langword="class"/>es to search for.</param>
    /// <returns>Every found <see langword="class"/> if the <paramref name="assembly"/> loads, none otherwise.</returns>
    public static IEnumerable<Type> FindLoadedClassTypes(Assembly? assembly)
    {
        if (assembly == null)
        {
            return [];
        }

        return FindLoadedTypes(assembly)
            .Where(t => t.IsClass)
            .Where(t => !t.IsNestedPrivate)
            .Where(t => !t.IsDefined(typeof(CompilerGeneratedAttribute), false));
    }

    /// <summary>Finds every <see cref="Type"/> in the <paramref name="assembly"/>.</summary>
    /// <param name="assembly"><see cref="Assembly"/> containing the <see cref="Type"/>s to search for.</param>
    /// <returns>Every found <see cref="Type"/> if the <paramref name="assembly"/> can load, none otherwise.</returns>
    internal static IEnumerable<Type> FindLoadedTypes(Assembly? assembly)
    {
        try
        {
            return assembly?.GetTypes() ?? Type.EmptyTypes;
        }
        catch
        {
            return Type.EmptyTypes;
        }
    }
}
