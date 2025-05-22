using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IEnumerable"/> collections for <see cref="IValuer"/>.</summary>
public sealed class EnumerableCompareHint : CompareHint<IEnumerable>
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IEnumerable? expected,
        IEnumerable? actual,
        ValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(expected, nameof(expected));
        ArgumentGuard.ThrowIfNull(actual, nameof(actual));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return LazyCompare(expected, actual, valuer);
    }

    /// <inheritdoc cref="Compare"/>
    private static IEnumerable<Difference> LazyCompare(
        IEnumerable expected,
        IEnumerable actual,
        ValuerChainer valuer
    )
    {
        if (valuer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }

        IEnumerator expectedEnumerator = expected.GetEnumerator();
        IEnumerator actualEnumerator = actual.GetEnumerator();
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

    /// <inheritdoc/>
    protected override Task<IEnumerable<Difference>> CompareAsync(
        IEnumerable? expected,
        IEnumerable? actual,
        ValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(expected, nameof(expected));
        ArgumentGuard.ThrowIfNull(actual, nameof(actual));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return LazyCompareAsync(expected, actual, valuer);
    }

    /// <inheritdoc cref="CompareAsync"/>
    private static async Task<IEnumerable<Difference>> LazyCompareAsync(
        IEnumerable expected,
        IEnumerable actual,
        ValuerChainer valuer
    )
    {
        List<Difference> results = [];

        if (valuer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            results.Add(new Difference(expected.GetType(), actual.GetType()));
        }

        IEnumerator expectedEnumerator = expected.GetEnumerator();
        IEnumerator actualEnumerator = actual.GetEnumerator();
        int index = 0;

        while (expectedEnumerator.MoveNext())
        {
            if (actualEnumerator.MoveNext())
            {
                foreach (
                    Difference diff in await valuer
                        .CompareAsync(expectedEnumerator.Current, actualEnumerator.Current)
                        .ConfigureAwait(false)
                )
                {
                    results.Add(new Difference(index, diff));
                }
            }
            else
            {
                results.Add(
                    new Difference(
                        index,
                        new Difference(expectedEnumerator.Current, "'outofbounds'")
                    )
                );
            }
            index++;
        }
        while (actualEnumerator.MoveNext())
        {
            results.Add(
                new Difference(index++, new Difference("'outofbounds'", actualEnumerator.Current))
            );
        }
        return results;
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IEnumerable? item, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item, nameof(item));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        int hash = ValueComparer.BaseHash;
        foreach (object value in item)
        {
            hash = hash * ValueComparer.HashMultiplier + valuer.GetHashCode(value);
        }
        return hash;
    }

    /// <inheritdoc/>
    protected override async Task<int> GetHashCodeAsync(IEnumerable? item, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item, nameof(item));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        int hash = ValueComparer.BaseHash;
        foreach (object value in item)
        {
            hash =
                hash * ValueComparer.HashMultiplier
                + await valuer.GetHashCodeAsync(value).ConfigureAwait(false);
        }
        return hash;
    }
}
