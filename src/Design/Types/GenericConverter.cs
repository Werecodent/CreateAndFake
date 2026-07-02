using System.Reflection;

namespace CreateAndFake.Design.Types;

/// <summary>Provides common <see cref="Type"/> patterns for identifying generics.</summary>
public static class GenericConverter
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
        static IEnumerable<Type> loopBaseTypes(Type? origin)
        {
            Type? current = origin;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }

        return Enumerable
            .Empty<Type>()
            .Concat(child?.GetInterfaces() ?? [])
            .Concat(loopBaseTypes(child))
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

    /// <summary>Builds a <see cref="Type"/> name with any generics included.</summary>
    /// <param name="instance">The instance to create a <see cref="Type"/> name for.</param>
    /// <returns>The built display name.</returns>
    public static string ExpandName(object? instance)
    {
        return ExpandName(instance is Type type ? type : instance?.GetType());
    }

    /// <summary>Builds a <typeparamref name="T"/> name with any generics included.</summary>
    /// <typeparam name="T">The <see cref="Type"/> to create a name for.</typeparam>
    /// <returns>The built display name.</returns>
    public static string ExpandName<T>()
    {
        return ExpandName(typeof(T));
    }

    /// <summary>Builds a <paramref name="type"/> name with any generics included.</summary>
    /// <param name="type">The <see cref="Type"/> to create a name for.</param>
    /// <returns>The built display name.</returns>
    public static string ExpandName(Type? type)
    {
        if (type?.IsGenericType == true)
        {
            return string.Concat(
                type.Name.Substring(0, type.Name.IndexOf("`", StringComparison.Ordinal)),
                "<",
                string.Join(",", type.GetGenericArguments().Select(ExpandName)),
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
                .Select(p => ExpandName(p.ParameterType));

            return $"{method.Name}({string.Join(",", paramNames)})";
        }
        else
        {
            return "";
        }
    }
}
