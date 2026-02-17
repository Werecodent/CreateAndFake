using System.Collections;
using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
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
        IValuerChainer valuer
    )
    {
        return LazyCompare(expected, actual, valuer);
    }

    /// <inheritdoc cref="Compare"/>
    private static IEnumerable<Difference> LazyCompare(
        IEnumerable expected,
        IEnumerable actual,
        IValuerChainer valuer
    )
    {
        if (valuer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
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

            while (expectedEnumerator.MoveNext())
            {
                if (actualEnumerator.MoveNext())
                {
                    foreach (
                        Difference diff in valuer.Compare(
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
                        new Difference(expectedEnumerator.Current, "'outofbounds'")
                    );
                }
                index++;
            }
            while (actualEnumerator.MoveNext())
            {
                yield return new Difference(
                    index++,
                    new Difference("'outofbounds'", actualEnumerator.Current)
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
        IValuerChainer valuer,
        [EnumeratorCancellation] CancellationToken canceler
    )
    {
        if (valuer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
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

            while (expectedEnumerator.MoveNext())
            {
                if (actualEnumerator.MoveNext())
                {
                    await foreach (
                        Difference diff in valuer
                            .CompareAsync(expectedEnumerator.Current, actualEnumerator.Current)
                            .WithCancellation(canceler)
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
                        new Difference(expectedEnumerator.Current, "'outofbounds'")
                    );
                }
                index++;
            }
            while (actualEnumerator.MoveNext())
            {
                yield return new Difference(
                    index++,
                    new Difference("'outofbounds'", actualEnumerator.Current)
                );
            }
        }
        finally
        {
            await Disposer.CleanupAsync(expectedEnumerator, actualEnumerator).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IEnumerable item, IValuerChainer valuer)
    {
        int hash = ValueComparer.BaseHash;
        foreach (object value in item)
        {
            hash = hash * ValueComparer.HashMultiplier + valuer.GetHashCode(value);
        }
        return hash;
    }

    /// <inheritdoc/>
    protected override async Task<int> GetHashCodeAsync(
        IEnumerable item,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        int hash = ValueComparer.BaseHash;
        foreach (object value in item)
        {
            hash =
                hash * ValueComparer.HashMultiplier
                + await valuer.GetHashCodeAsync(value, canceler).ConfigureAwait(false);
        }
        return hash;
    }
}
