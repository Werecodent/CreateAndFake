using System.Collections;
using System.Runtime.CompilerServices;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IEnumerable"/> collections for <see cref="IValuer"/>.</summary>
public sealed class EnumerableCompareHint : CompareHint<IEnumerable>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.EnumerableHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IEnumerable expected,
        IEnumerable actual,
        IValuerChainer chainer
    )
    {
        return LazyCompare(expected, actual, chainer);
    }

    /// <inheritdoc cref="Compare"/>
    private static IEnumerable<Difference> LazyCompare(
        IEnumerable expected,
        IEnumerable actual,
        IValuerChainer chainer
    )
    {
        if (chainer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }

        IEnumerator? expectedEnumerator = null;
        IEnumerator? actualEnumerator = null;
        try
        {
            expectedEnumerator = expected.GetEnumerator();
            actualEnumerator = actual.GetEnumerator();
            int index = 0;

            while (expectedEnumerator.MoveNext() && index < chainer.Options.IterationLimit)
            {
                if (actualEnumerator.MoveNext())
                {
                    foreach (
                        Difference diff in chainer.Compare(
                            expectedEnumerator.Current,
                            actualEnumerator.Current
                        )
                    )
                    {
                        yield return new Difference(index, diff);
                    }
                }
                else
                {
                    yield return new Difference(
                        index,
                        new Difference(expectedEnumerator.Current, "'out of range'")
                    );
                }
                index++;
            }
            while (actualEnumerator.MoveNext() && index < chainer.Options.IterationLimit)
            {
                yield return new Difference(
                    index++,
                    new Difference("'out of range'", actualEnumerator.Current)
                );
            }

            if (index >= chainer.Options.IterationLimit)
            {
                throw new EngineException(
                    $"Reached {nameof(IEnumerable)} max iteration limit "
                        + $"({index}) from {nameof(ValuerOptions.IterationLimit)}."
                );
            }
        }
        finally
        {
            Disposer.Cleanup(expectedEnumerator, actualEnumerator);
        }
    }

    /// <inheritdoc/>
    protected override async IAsyncEnumerable<Difference> CompareAsync(
        IEnumerable expected,
        IEnumerable actual,
        IValuerChainer chainer,
        [EnumeratorCancellation] CancellationToken canceler
    )
    {
        if (chainer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }

        IEnumerator? expectedEnumerator = null;
        IEnumerator? actualEnumerator = null;
        try
        {
            expectedEnumerator = expected.GetEnumerator();
            actualEnumerator = actual.GetEnumerator();
            int index = 0;

            while (expectedEnumerator.MoveNext() && index < chainer.Options.IterationLimit)
            {
                if (actualEnumerator.MoveNext())
                {
                    await foreach (
                        Difference diff in chainer
                            .CompareAsync(
                                expectedEnumerator.Current,
                                actualEnumerator.Current,
                                canceler
                            )
                            .ConfigureAwait(false)
                    )
                    {
                        yield return new Difference(index, diff);
                    }
                }
                else
                {
                    yield return new Difference(
                        index,
                        new Difference(expectedEnumerator.Current, "'out of range'")
                    );
                }
                index++;
            }
            while (actualEnumerator.MoveNext() && index < chainer.Options.IterationLimit)
            {
                yield return new Difference(
                    index++,
                    new Difference("'out of range'", actualEnumerator.Current)
                );
            }

            if (index >= chainer.Options.IterationLimit)
            {
                throw new EngineException(
                    $"Reached {nameof(IEnumerable)} max iteration limit "
                        + $"({index}) from {nameof(ValuerOptions.IterationLimit)}."
                );
            }
        }
        finally
        {
            await Disposer.CleanupAsync(expectedEnumerator, actualEnumerator).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IEnumerable item, IValuerChainer chainer)
    {
        int index = 0;
        int hash = ValueComparer.BaseHash;
        foreach (object value in item)
        {
            if (index++ >= chainer.Options.IterationLimit)
            {
                throw new EngineException(
                    $"Reached {nameof(IEnumerable)} max iteration limit "
                        + $"({index}) from {nameof(ValuerOptions.IterationLimit)}."
                );
            }

            hash = hash * ValueComparer.HashMultiplier + chainer.GetHashCode(value);
        }
        return hash;
    }

    /// <inheritdoc/>
    protected override async Task<int> GetHashCodeAsync(
        IEnumerable item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        int index = 0;
        int hash = ValueComparer.BaseHash;
        foreach (object value in item)
        {
            if (index++ >= chainer.Options.IterationLimit)
            {
                throw new EngineException(
                    $"Reached {nameof(IEnumerable)} max iteration limit ({index}) "
                        + $"from {nameof(ValuerOptions.IterationLimit)}."
                );
            }

            hash =
                hash * ValueComparer.HashMultiplier
                + await chainer.GetHashCodeAsync(value, canceler).ConfigureAwait(false);
        }
        return hash;
    }
}
