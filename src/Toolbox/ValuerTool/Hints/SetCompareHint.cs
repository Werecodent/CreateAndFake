using System.Collections;
using System.Runtime.CompilerServices;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Hints;

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

    /// <inheritdoc cref="Compare"/>
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

        AsyncHashSet<object> expectedByHash = AsyncHashSet<object>.CreateFromAsync(
            expected.Cast<object>(),
            chainer,
            chainer.Options.IterationLimit,
            canceler
        );
        AsyncHashSet<object> actualByHash = AsyncHashSet<object>.CreateFromAsync(
            expected.Cast<object>(),
            chainer,
            chainer.Options.IterationLimit,
            canceler
        );

        await foreach (
            object item in expectedByHash
                .FindMissingFromAsync(actualByHash, canceler)
                .ConfigureAwait(false)
        )
        {
            yield return new Difference(item, "'missing'");
        }

        await foreach (
            object item in actualByHash
                .FindMissingFromAsync(expectedByHash, canceler)
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
