using System.Collections;
using System.Runtime.CompilerServices;
using CreateAndFake.Design.Comparisons;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Types;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <inheritdoc/>
public sealed class SetCompareHint : CompareHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.SetHint;

    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer chainer)
    {
        return expected.GetType().Inherits(typeof(ISet<>))
            && actual.GetType().Inherits(typeof(ISet<>));
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object expected,
        object actual,
        IValuerChainer chainer
    )
    {
        Type expectedType = GenericConverter.FindConcreteType(expected.GetType(), typeof(ISet<>));
        Type actualType = GenericConverter.FindConcreteType(actual.GetType(), typeof(ISet<>));

        if (expectedType != actualType)
        {
            return [new Difference(expected.GetType(), actual.GetType())];
        }

        return HandleCompare((IEnumerable)expected, (IEnumerable)actual, chainer);
    }

    /// <inheritdoc/>
    private static IEnumerable<Difference> HandleCompare(
        IEnumerable expected,
        IEnumerable actual,
        IValuerChainer chainer
    )
    {
        if (chainer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }

        HashSet<object> expectedByValues = new(expected.Cast<object>(), chainer);
        HashSet<object> actualByValues = new(actual.Cast<object>(), chainer);

        foreach (object item in expectedByValues)
        {
            if (!actualByValues.Contains(item))
            {
                yield return new Difference(
                    $"[{TryDescribe(item)}]",
                    new Difference(item, "'missing'")
                );
            }
        }

        foreach (object item in actualByValues)
        {
            if (!expectedByValues.Contains(item))
            {
                yield return new Difference(
                    $"[{TryDescribe(item)}]",
                    new Difference("'missing'", item)
                );
            }
        }
    }

    /// <inheritdoc/>
    protected override IAsyncEnumerable<Difference> CompareAsync(
        object expected,
        object actual,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        Type expectedType = GenericConverter.FindConcreteType(expected.GetType(), typeof(ISet<>));
        Type actualType = GenericConverter.FindConcreteType(actual.GetType(), typeof(ISet<>));

        if (expectedType != actualType)
        {
            return AsyncSeriesHelper.CreateFromAsync(
                [new Difference(expected.GetType(), actual.GetType())],
                chainer.Options.IterationLimit,
                canceler
            );
        }

        return HandleCompareAsync((dynamic)expected, (dynamic)actual, chainer, canceler);
    }

    /// <inheritdoc cref="CompareAsync"/>
    private static async IAsyncEnumerable<Difference> HandleCompareAsync(
        IEnumerable expected,
        IEnumerable actual,
        IValuerChainer chainer,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        canceler.ThrowIfCancellationRequested();
        if (chainer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }

        async Task<Dictionary<int, List<object>>> byHashes(IEnumerable set)
        {
            Dictionary<int, List<object>> result = [];
            foreach (object item in set)
            {
                int valueHash = await chainer
                    .GetHashCodeAsync(item, canceler)
                    .ConfigureAwait(false);

                if (result.TryGetValue(valueHash, out List<object>? values))
                {
                    values.Add(item);
                }
                else
                {
                    result[valueHash] = [item];
                }
            }
            return result;
        }

        async IAsyncEnumerable<object> findMissing(
            Dictionary<int, List<object>> set,
            Dictionary<int, List<object>> other
        )
        {
            foreach (KeyValuePair<int, List<object>> series in set)
            {
                List<object> matches = other.TryGetValue(series.Key, out List<object>? value)
                    ? value
                    : [];
                foreach (object setItem in series.Value)
                {
                    bool notFound = true;
                    foreach (object otherItem in matches)
                    {
                        canceler.ThrowIfCancellationRequested();
                        if (
                            await chainer
                                .EqualsAsync(setItem, otherItem, canceler)
                                .ConfigureAwait(false)
                        )
                        {
                            notFound = false;
                            break;
                        }
                    }
                    if (notFound)
                    {
                        yield return setItem;
                    }
                }
            }
        }

        Dictionary<int, List<object>> expectedByHash = await byHashes(expected)
            .ConfigureAwait(false);
        Dictionary<int, List<object>> actualByHash = await byHashes(actual).ConfigureAwait(false);

        await foreach (
            object item in findMissing(expectedByHash, actualByHash)
                .WithCancellation(canceler)
                .ConfigureAwait(false)
        )
        {
            yield return new Difference(item, "'missing'");
        }

        await foreach (
            object item in findMissing(expectedByHash, actualByHash)
                .WithCancellation(canceler)
                .ConfigureAwait(false)
        )
        {
            yield return new Difference("'missing'", item);
        }
    }

    /// <summary>Expands the <paramref name="item"/>'s name if it's a <see cref="Type"/>.</summary>
    /// <param name="item">Potential <see cref="Type"/> to describe.</param>
    /// <returns>
    ///     Description of the <paramref name="item"/> if it's a <see cref="Type"/>,
    ///     the <paramref name="item"/> otherwise.
    /// </returns>
    private static object? TryDescribe(object? item)
    {
        return (item is Type type) ? GenericConverter.ExpandName(type) : item;
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object item, IValuerChainer chainer)
    {
        int hash = ValueComparer.BaseHash;
        foreach (object entry in ((IEnumerable)item).Cast<object>())
        {
            hash += chainer.GetHashCode(entry);
        }
        return hash;
    }

    /// <inheritdoc/>
    protected override async Task<int> GetHashCodeAsync(
        object item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        int hash = ValueComparer.BaseHash;
        foreach (object entry in ((IEnumerable)item).Cast<object>())
        {
            canceler.ThrowIfCancellationRequested();
            hash += await chainer.GetHashCodeAsync(entry, canceler).ConfigureAwait(false);
        }

        canceler.ThrowIfCancellationRequested();
        return hash;
    }
}
