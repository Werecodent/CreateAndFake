using System.Collections.Frozen;
using Werecodent.CreateAndFake.Design.Extensions;

namespace Werecodent.CreateAndFake.Design.Types;

/// <summary>Provides common <see cref="ITypeSupporter"/> patterns.</summary>
public static class TypeSupporter
{
    /// <summary>
    ///     Groups the <paramref name="typeHandlers"/> by their <see cref="ITypeSupporter.SupportedType"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="ITypeSupporter"/> <see cref="Type"/> being grouped.</typeparam>
    /// <param name="typeHandlers">The collection to group via iteration.</param>
    /// <returns>
    ///     The collected <paramref name="typeHandlers"/> keyed by their <see cref="ITypeSupporter.SupportedType"/>.
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
    ///     Groups the <paramref name="typeHandlers"/> by what the <see cref="ITypeSupporter.SupportedType"/> inherits.
    /// </summary>
    /// <typeparam name="T">The <see cref="ITypeSupporter"/> <see cref="Type"/> being grouped.</typeparam>
    /// <param name="typeHandlers">The collection to group via iteration.</param>
    /// <returns>
    ///     The collected <paramref name="typeHandlers"/> keyed by every <see langword="class"/> &amp;
    ///     <see langword="interface"/> their <see cref="ITypeSupporter.SupportedType"/> inherits.
    /// </returns>
    public static IDictionary<Type, T[]> GroupByInheritance<T>(IEnumerable<T> typeHandlers)
        where T : ITypeSupporter
    {
        Dictionary<Type, IList<T>> results = [];
        foreach (T handler in typeHandlers ?? [])
        {
            foreach (
                Type type in TypeDescriber
                    .For(handler.SupportedType)
                    .InheritedTypes.Where(t => !t.IsGenericTypeDefinition)
                    .Where(t => !t.Inherits<Delegate>() || t == handler.SupportedType)
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

    /// <summary>
    ///     Groups the <paramref name="typeHandlers"/> by what the
    ///     <see cref="ITypeSupporter.SupportedType"/> is inherited by.
    /// </summary>
    /// <typeparam name="T">The <see cref="ITypeSupporter"/> <see cref="Type"/> being grouped.</typeparam>
    /// <param name="typeHandlers">The collection to group via iteration.</param>
    /// <returns>
    ///     The collected <paramref name="typeHandlers"/> keyed by every <see langword="class"/> &amp;
    ///     <see langword="interface"/> their <see cref="ITypeSupporter.SupportedType"/> is inherited by.
    /// </returns>
    public static IDictionary<Type, T[]> GroupBySubclasses<T>(IEnumerable<T> typeHandlers)
        where T : ITypeSupporter
    {
        Dictionary<Type, IList<T>> results = [];
        foreach (T handler in typeHandlers ?? [])
        {
            foreach (Type type in TypeDescriber.For(handler.SupportedType).FindLoadedSubclasses())
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
