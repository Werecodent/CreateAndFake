using System.Collections.Frozen;

namespace CreateAndFake.Design.Content;

/// <summary>Common manipulations for <see cref="ITypeSupporter"/> classes.</summary>
public static class TypeSupporter
{
    /// <summary>
    ///     Collects <paramref name="typeHandlers"/> into a dictionary by their supported type.
    /// </summary>
    /// <typeparam name="T">Collection type being grouped.</typeparam>
    /// <param name="typeHandlers">Collection to group.</param>
    /// <returns>The grouped result.</returns>
    public static IDictionary<Type, T> GroupBySupportedType<T>(IEnumerable<T> typeHandlers)
        where T : ITypeSupporter
    {
        Dictionary<Type, T> results = [];
        foreach (T handler in typeHandlers ?? [])
        {
            results.Add(handler.SupportedType, handler);
        }
        return results.ToFrozenDictionary(p => p.Key, p => p.Value);
    }

    /// <summary>
    ///     Collects <paramref name="typeHandlers"/> into a
    ///     dictionary by their supported type's inheritance tree.
    /// </summary>
    /// <typeparam name="T">Collection type being grouped.</typeparam>
    /// <param name="typeHandlers">Collection to group.</param>
    /// <returns>The grouped result.</returns>
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
