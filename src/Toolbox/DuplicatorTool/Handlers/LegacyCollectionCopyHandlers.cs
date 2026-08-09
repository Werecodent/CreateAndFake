using System.Collections;
using System.Collections.Specialized;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.DuplicatorTool.Handlers;

/// <summary>Holds a collection of related handlers.</summary>
internal static class LegacyCollectionCopyHandlers
{
    /// <summary>The collection of related handlers.</summary>
    internal static IEnumerable<ICopyHandler> Handlers { get; } =
    [
        new FactoryCopyHandler<ArrayList>(CreateAndCopyList),
        new FactoryCopyHandler<Hashtable>(CreateAndCopyDictionary),
        new FactoryCopyHandler<SortedList>(CreateAndCopyDictionary),
        new FactoryCopyHandler<StringCollection>((data, _) => [.. data]),
        new FactoryCopyHandler<BitArray>((data, _) => new BitArray(data)),
        new FactoryCopyHandler<ListDictionary>(CreateAndCopyDictionary),
        new FactoryCopyHandler<HybridDictionary>(CreateAndCopyDictionary),
        new FactoryCopyHandler<NameValueCollection>((data, _) => new NameValueCollection(data)),
        new FactoryCopyHandler<Queue>(
            (data, copier) => new Queue(data.Cast<object>().Select(d => copier.Copy(d)).ToArray())
        ),
        new FactoryCopyHandler<Stack>(
            (data, copier) =>
                new Stack(data.Cast<object>().Select(d => copier.Copy(d)).Reverse().ToArray())
        ),
        new FactoryCopyHandler<StringDictionary>(
            (data, _) =>
            {
                StringDictionary result = [];
                foreach (DictionaryEntry entry in data)
                {
                    result.Add((string)entry.Key, (string?)entry.Value);
                }
                return result;
            }
        ),
        new FactoryCopyHandler<OrderedDictionary>(
            (data, duplicator) =>
            {
                OrderedDictionary result = [];

                Dictionary<object, object?> transfer = [];
                foreach (DictionaryEntry entry in data)
                {
                    transfer.Add(entry.Key, entry.Value);
                }

                for (int i = 0; i < data.Count; i++)
                {
                    object? value = data[i];
                    object key = transfer.First(entry => ReferenceEquals(entry.Value, value)).Key;
                    _ = transfer.Remove(key);

                    result.Add(duplicator.Copy(key), duplicator.Copy(value));
                }
                return result;
            }
        ),
    ];

    /// <summary>Clones <paramref name="source"/>.</summary>
    /// <typeparam name="T">Collection type being cloned.</typeparam>
    /// <param name="source">Collection to clone.</param>
    /// <param name="duplicator">Handles callback behavior for child values.</param>
    /// <returns>Clone of <paramref name="source"/>.</returns>
    private static T CreateAndCopyList<T>(T source, IDuplicatorChainer duplicator)
        where T : IList, new()
    {
        T result = new();
        foreach (object item in source)
        {
            _ = result.Add(duplicator.Copy(item));
        }
        return result;
    }

    /// <inheritdoc cref="CreateAndCopyList"/>
    private static T CreateAndCopyDictionary<T>(T source, IDuplicatorChainer duplicator)
        where T : IDictionary, new()
    {
        T result = new();
        foreach (DictionaryEntry entry in source)
        {
            result.Add(duplicator.Copy(entry.Key), duplicator.Copy(entry.Value));
        }
        return result;
    }
}
