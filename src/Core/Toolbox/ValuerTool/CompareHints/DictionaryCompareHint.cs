using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;

namespace CreateAndFake.Toolbox.ValuerTool.CompareHints;

/// <summary>Handles comparing <see cref="IDictionary"/> collections for <see cref="IValuer"/>.</summary>
public sealed class DictionaryCompareHint : CompareHint<IDictionary>
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IDictionary? expected, IDictionary? actual, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(expected, nameof(expected));
        ArgumentGuard.ThrowIfNull(actual, nameof(actual));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return LazyCompare(expected, actual, valuer);
    }

    /// <inheritdoc cref="Compare"/>
    private static IEnumerable<Difference> LazyCompare(
        IDictionary expected, IDictionary actual, ValuerChainer valuer)
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
    protected override int GetHashCode(IDictionary? item, ValuerChainer valuer)
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
}
