using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IDictionary"/> collections for <see cref="IValuer"/>.</summary>
public sealed class DictionaryCompareHint : CompareHint<IDictionary>
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IDictionary? expected,
        IDictionary? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(expected, nameof(expected));
        ArgumentGuard.ThrowIfNull(actual, nameof(actual));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return LazyCompare(expected, actual, valuer);
    }

    /// <inheritdoc cref="Compare"/>
    private static IEnumerable<Difference> LazyCompare(
        IDictionary expected,
        IDictionary actual,
        IValuerChainer valuer
    )
    {
        if (valuer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }

        object[] expectedKeys = [.. expected.Keys.Cast<object>()];
        object[] actualKeys = [.. actual.Keys.Cast<object>()];

        foreach (object key in expectedKeys)
        {
            object? match = actualKeys.FirstOrDefault(k => valuer.Equals(key, k));
            if (match != null)
            {
                foreach (Difference diff in valuer.Compare(expected[key], actual[match]))
                {
                    yield return new Difference($"[{key}]", diff);
                }
            }
            else
            {
                yield return new Difference($"[{key}]", new Difference(expected[key], "'null'"));
            }
        }

        foreach (object key in actualKeys)
        {
            if (!expectedKeys.Any(k => valuer.Equals(key, k)))
            {
                yield return new Difference($"[{key}]", new Difference("'null'", actual[key]));
            }
        }
    }

    /// <inheritdoc/>
    protected override Task<IEnumerable<Difference>> CompareAsync(
        IDictionary? expected,
        IDictionary? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(expected, nameof(expected));
        ArgumentGuard.ThrowIfNull(actual, nameof(actual));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return LazyCompareAsync(expected, actual, valuer);
    }

    /// <inheritdoc cref="CompareAsync"/>
    private static async Task<IEnumerable<Difference>> LazyCompareAsync(
        IDictionary expected,
        IDictionary actual,
        IValuerChainer valuer
    )
    {
        List<Difference> results = [];

        if (valuer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            results.Add(new Difference(expected.GetType(), actual.GetType()));
        }

        object[] expectedKeys = [.. expected.Keys.Cast<object>()];
        object[] actualKeys = [.. actual.Keys.Cast<object>()];

        foreach (object key in expectedKeys)
        {
            object? match = null;
            foreach (object potentialMatch in actualKeys)
            {
                if (await valuer.EqualsAsync(key, potentialMatch).ConfigureAwait(false))
                {
                    match = potentialMatch;
                    break;
                }
            }

            if (match != null)
            {
                foreach (
                    Difference diff in await valuer
                        .CompareAsync(expected[key], actual[match])
                        .ConfigureAwait(false)
                )
                {
                    results.Add(new Difference($"[{key}]", diff));
                }
            }
            else
            {
                results.Add(new Difference($"[{key}]", new Difference(expected[key], "'null'")));
            }
        }

        foreach (object key in actualKeys)
        {
            object? match = null;
            foreach (object potentialMatch in expectedKeys)
            {
                if (await valuer.EqualsAsync(key, potentialMatch).ConfigureAwait(false))
                {
                    match = potentialMatch;
                    break;
                }
            }

            if (match == null)
            {
                results.Add(new Difference($"[{key}]", new Difference("'null'", actual[key])));
            }
        }

        return results;
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IDictionary? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item, nameof(item));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        int hash = ValueComparer.BaseHash;
        foreach (DictionaryEntry entry in item)
        {
            hash += valuer.GetHashCode(entry);
        }
        return hash;
    }

    /// <inheritdoc/>
    protected override async Task<int> GetHashCodeAsync(IDictionary? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item, nameof(item));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        int hash = ValueComparer.BaseHash;
        foreach (DictionaryEntry entry in item)
        {
            hash += await valuer.GetHashCodeAsync(entry).ConfigureAwait(false);
        }
        return hash;
    }
}
