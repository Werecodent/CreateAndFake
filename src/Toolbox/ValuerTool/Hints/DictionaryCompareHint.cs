using System.Collections;
using System.Runtime.CompilerServices;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Hints;

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

        HashSet<object> expectedKeys = new(expected.Keys.Cast<object>(), chainer);
        Dictionary<object, object> actualKeys = actual
            .Keys.Cast<object>()
            .ToDictionary(e => e, e => e, chainer);

        foreach (object key in expectedKeys)
        {
            if (actualKeys.TryGetValue(key, out object? match))
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
                    new Difference(expected[key], "'missing'")
                );
            }
        }

        foreach (object key in actualKeys.Keys)
        {
            if (!expectedKeys.Contains(key))
            {
                yield return new Difference(
                    $"[{TryDescribe(key)}]",
                    new Difference("'missing'", actual[key])
                );
            }
        }
    }

    /// <inheritdoc/>
    protected override IAsyncEnumerable<Difference> CompareAsync(
        IDictionary expected,
        IDictionary actual,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        return HandleCompareAsync(expected, actual, chainer, canceler);
    }

    /// <inheritdoc cref="CompareAsync"/>
    private static async IAsyncEnumerable<Difference> HandleCompareAsync(
        IDictionary expected,
        IDictionary actual,
        IValuerChainer chainer,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        canceler.ThrowIfCancellationRequested();
        if (chainer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }

        HashSet<object> expectedKeys = new(expected.Keys.Cast<object>(), chainer);
        Dictionary<object, object> actualKeys = actual
            .Keys.Cast<object>()
            .ToDictionary(e => e, e => e, chainer);

        foreach (object key in expectedKeys)
        {
            if (actualKeys.TryGetValue(key, out object? match))
            {
                await foreach (
                    Difference diff in chainer
                        .CompareAsync(expected[key], actual[match], canceler)
                        .ConfigureAwait(false)
                )
                {
                    yield return new Difference($"[{TryDescribe(key)}]", diff);
                    canceler.ThrowIfCancellationRequested();
                }
            }
            else
            {
                yield return new Difference(
                    $"[{TryDescribe(key)}]",
                    new Difference(expected[key], "'missing'")
                );
            }
        }

        foreach (object key in actualKeys.Keys)
        {
            if (!expectedKeys.Contains(key))
            {
                yield return new Difference(
                    $"[{TryDescribe(key)}]",
                    new Difference("'missing'", actual[key])
                );
            }
        }
    }

    /// <summary>Expands the <paramref name="item"/>'s name if it's a <see cref="Type"/>.</summary>
    /// <param name="item">Potential <see cref="Type"/> to describe.</param>
    /// <returns>
    ///     Description of the <paramref name="item"/> if it's a <see cref="Type"/>,
    ///     the <paramref name="item"/> otherwise.
    /// </returns>
    private static object TryDescribe(object item)
    {
        return (item is Type type) ? GenericConverter.ExpandName(type) : item;
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IDictionary item, IValuerChainer chainer)
    {
        int hash = ValueComparer.BaseHash;
        foreach (DictionaryEntry entry in item)
        {
            hash += chainer.GetHashCode(entry.Key);
            hash += chainer.GetHashCode(entry.Value);
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
            canceler.ThrowIfCancellationRequested();
            hash += chainer.GetHashCode(entry.Key);
            hash += await chainer.GetHashCodeAsync(entry.Value, canceler).ConfigureAwait(false);
        }

        canceler.ThrowIfCancellationRequested();
        return hash;
    }
}
