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
        { typeof(OrderedDictionary), CreateAndCopy<OrderedDictionary> },
        { typeof(BitArray), (data, _) => new BitArray((BitArray)data) },
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
