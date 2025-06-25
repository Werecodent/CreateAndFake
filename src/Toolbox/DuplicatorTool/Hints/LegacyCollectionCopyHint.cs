using System.Collections;
using System.Collections.Frozen;
using System.Collections.Specialized;
using CreateAndFake.Design;
using CreateAndFake.DuplicatorTool.Engine;

#pragma warning disable IDE0058 // Return not available on all versions.
#pragma warning disable RCS1124 // Inlining creates the wrong type.

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning legacy collections for <see cref="IDuplicator"/> .</summary>
public sealed class LegacyCollectionCopyHint : CopyHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly FrozenDictionary<
        Type,
        Func<object, IDuplicatorChainer, object>
    > _Copiers = new Dictionary<Type, Func<object, IDuplicatorChainer, object>>()
    {
        { typeof(Hashtable), CreateAndCopy<Hashtable> },
        { typeof(SortedList), CreateAndCopy<SortedList> },
        { typeof(ListDictionary), CreateAndCopy<ListDictionary> },
        { typeof(HybridDictionary), CreateAndCopy<HybridDictionary> },
        { typeof(BitArray), (data, _) => new BitArray((BitArray)data) },
        {
            typeof(OrderedDictionary),
            (data, duplicator) =>
            {
                OrderedDictionary original = (OrderedDictionary)data;
                OrderedDictionary result = [];

                Dictionary<object, object?> transfer = [];
                foreach (DictionaryEntry entry in original)
                {
                    transfer.Add(entry.Key, entry.Value);
                }

                for (int i = 0; i < original.Count; i++)
                {
                    object? value = original[i];
                    object key = transfer.First(entry => ReferenceEquals(entry.Value, value)).Key;
                    transfer.Remove(key);

                    result.Add(duplicator.Copy(key), duplicator.Copy(value));
                }
                return result;
            }
        },
        {
            typeof(NameValueCollection),
            (data, _) => new NameValueCollection((NameValueCollection)data)
        },
        {
            typeof(StringCollection),
            (data, _) =>
            {
                StringCollection result = [.. (StringCollection)data];
                return result;
            }
        },
        {
            typeof(StringDictionary),
            (data, _) =>
            {
                StringDictionary result = [];
                foreach (DictionaryEntry entry in (StringDictionary)data)
                {
                    result.Add((string)entry.Key, (string?)entry.Value);
                }
                return result;
            }
        },
    }.ToFrozenDictionary();

    /// <inheritdoc/>
    public sealed override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, nameof(source));
        ArgumentGuard.ThrowIfNull(duplicator, nameof(duplicator));

        if (
            _Copiers.TryGetValue(
                source.GetType(),
                out Func<object, IDuplicatorChainer, object>? copier
            )
        )
        {
            return new(copier.Invoke(source, duplicator));
        }
        else
        {
            return CopyHintResult.None;
        }
    }

    /// <summary>Clones <paramref name="source"/>.</summary>
    /// <typeparam name="T">Collection type being cloned.</typeparam>
    /// <param name="source">Collection to clone.</param>
    /// <param name="duplicator">Handles callback behavior for child values.</param>
    /// <returns>Clone of <paramref name="source"/>.</returns>
    private static T CreateAndCopy<T>(object source, IDuplicatorChainer duplicator)
        where T : IDictionary, new()
    {
        T result = new();
        foreach (DictionaryEntry entry in (T)source)
        {
            result.Add(duplicator.Copy(entry.Key)!, duplicator.Copy(entry.Value));
        }
        return result;
    }
}

#pragma warning restore IDE0058, RCS1124
