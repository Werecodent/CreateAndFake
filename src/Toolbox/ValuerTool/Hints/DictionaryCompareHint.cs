using System.Collections;
using System.Runtime.CompilerServices;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <inheritdoc/>
public sealed class DictionaryCompareHint : CompareHint<IDictionary>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.DictionaryHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IDictionary expected,
        IDictionary actual,
        IValuerChainer chainer
    )
    {
        if (chainer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }

        object[] expectedKeys = [.. expected.Keys.Cast<object>()];
        object[] actualKeys = [.. actual.Keys.Cast<object>()];

        foreach (object key in expectedKeys)
        {
            object? match = actualKeys.FirstOrDefault(k => chainer.Equals(key, k));
            if (match != null)
            {
                foreach (Difference diff in chainer.Compare(expected[key], actual[match]))
                {
                    yield return new Difference($"[{TryDescribe(key)}]", diff);
                }
            }
            else
            {
                yield return new Difference(
                    $"[{TryDescribe(key)}]",
                    new Difference(expected[key], "'null'")
                );
            }
        }

        foreach (object key in actualKeys)
        {
            if (!expectedKeys.Any(k => chainer.Equals(key, k)))
            {
                yield return new Difference(
                    $"[{TryDescribe(key)}]",
                    new Difference("'null'", actual[key])
                );
            }
        }
    }

    /// <inheritdoc/>
    protected override async IAsyncEnumerable<Difference> CompareAsync(
        IDictionary expected,
        IDictionary actual,
        IValuerChainer chainer,
        [EnumeratorCancellation] CancellationToken canceler
    )
    {
        if (chainer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }

        object[] expectedKeys = [.. expected.Keys.Cast<object>()];
        object[] actualKeys = [.. actual.Keys.Cast<object>()];

        foreach (object key in expectedKeys)
        {
            object? match = null;
            foreach (object potentialMatch in actualKeys)
            {
                if (await chainer.EqualsAsync(key, potentialMatch, canceler).ConfigureAwait(false))
                {
                    match = potentialMatch;
                    break;
                }
            }

            if (match != null)
            {
                await foreach (
                    Difference diff in chainer
                        .CompareAsync(expected[key], actual[match], canceler)
                        .WithCancellation(canceler)
                        .ConfigureAwait(false)
                )
                {
                    yield return new Difference($"[{TryDescribe(key)}]", diff);
                }
            }
            else
            {
                yield return new Difference(
                    $"[{TryDescribe(key)}]",
                    new Difference(expected[key], "'null'")
                );
            }
        }

        foreach (object key in actualKeys)
        {
            object? match = null;
            foreach (object potentialMatch in expectedKeys)
            {
                if (await chainer.EqualsAsync(key, potentialMatch, canceler).ConfigureAwait(false))
                {
                    match = potentialMatch;
                    break;
                }
            }

            if (match == null)
            {
                yield return new Difference(
                    $"[{TryDescribe(key)}]",
                    new Difference("'null'", actual[key])
                );
            }
        }
    }

    private static object TryDescribe(object item)
    {
        return (item is Type type) ? TypeDescriber.ExpandedName(type) : item;
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IDictionary item, IValuerChainer chainer)
    {
        int hash = ValueComparer.BaseHash;
        foreach (DictionaryEntry entry in item)
        {
            hash += chainer.GetHashCode(entry);
        }
        return hash;
    }

    /// <inheritdoc/>
    protected override async Task<int> GetHashCodeAsync(
        IDictionary item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        int hash = ValueComparer.BaseHash;
        foreach (DictionaryEntry entry in item)
        {
            hash += await chainer.GetHashCodeAsync(entry, canceler).ConfigureAwait(false);
        }
        return hash;
    }
}
