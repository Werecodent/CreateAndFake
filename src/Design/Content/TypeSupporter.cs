using System.Collections.Frozen;

namespace CreateAndFake.Design.Content;

/// <summary>Provides common <see cref="ITypeSupporter"/> patterns.</summary>
public static class TypeSupporter
{
    /// <summary>
    ///     Groups the <paramref name="typeHandlers"/> by
    ///     their <see cref="ITypeSupporter.SupportedType"/>.
    /// </summary>
    /// <typeparam name="T">
    ///     The <see cref="ITypeSupporter"/> <see cref="Type"/> being grouped.
    /// </typeparam>
    /// <param name="typeHandlers">The collection to group via iteration.</param>
    /// <returns>
    ///     The collected <paramref name="typeHandlers"/> keyed
    ///     by their <see cref="ITypeSupporter.SupportedType"/>.
    /// </returns>
    public static IDictionary<Type, T> GroupBySupportedType<T>(IEnumerable<T> typeHandlers)
        where T : ITypeSupporter
    {
        Dictionary<Type, T> results = [];
        foreach (T handler in typeHandlers ?? [])
        {
            if (handler.SupportedType != null)
            {
                results.Add(handler.SupportedType, handler);
            }
        }
        return results.ToFrozenDictionary(p => p.Key, p => p.Value);
    }

    /// <summary>
    ///     Groups the <paramref name="typeHandlers"/> by their
    ///     <see cref="ITypeSupporter.SupportedType"/>'s inheritance tree.
    /// </summary>
    /// <typeparam name="T">
    ///     The <see cref="ITypeSupporter"/> <see cref="Type"/> being grouped.
    /// </typeparam>
    /// <param name="typeHandlers">The collection to group via iteration.</param>
    /// <returns>
    ///     The collected <paramref name="typeHandlers"/> keyed by
    ///     every <see langword="class"/> &amp; <see langword="interface"/>
    ///     their <see cref="ITypeSupporter.SupportedType"/> inherits.
    /// </returns>
    public static IDictionary<Type, T[]> GroupByInheritance<T>(IEnumerable<T> typeHandlers)
        where T : ITypeSupporter
    {
        Dictionary<Type, IList<T>> results = [];
        foreach (T handler in typeHandlers ?? [])
        {
            foreach (
                Type type in InheritanceTracker
                    .For(handler.SupportedType)
                    .InheritedTypes.Where(t => !t.IsGenericTypeDefinition)
            )
            {
                if (results.TryGetValue(type, out IList<T>? values))
                {
                    values.Add(handler);
                }
                else
                {
                    results.Add(type, [handler]);
                }
            }
        }
        return results.ToFrozenDictionary(p => p.Key, p => p.Value.ToArray());
    }
}
