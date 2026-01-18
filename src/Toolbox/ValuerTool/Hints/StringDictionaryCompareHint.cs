using System.Collections;
using System.Collections.Specialized;
using CreateAndFake.Design;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="StringDictionary"/> collections for <see cref="IValuer"/>.</summary>
public sealed class StringDictionaryCompareHint : CompareHint<StringDictionary>
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        StringDictionary? expected,
        StringDictionary? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return valuer.Compare(Convert(expected), Convert(actual));
    }

    /// <inheritdoc/>
    protected override int GetHashCode(StringDictionary? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return valuer.GetHashCode(Convert(item));
    }

    /// <summary>Handles changing string dictionaries to dictionaries.</summary>
    /// <param name="dict">Dictionary to convert.</param>
    /// <returns>The converted dictionary.</returns>
    private static Dictionary<string, string?> Convert(StringDictionary? dict)
    {
        ArgumentGuard.ThrowIfNull(dict);

        return dict.Cast<DictionaryEntry>().ToDictionary(e => (string)e.Key, e => (string?)e.Value);
    }
}
