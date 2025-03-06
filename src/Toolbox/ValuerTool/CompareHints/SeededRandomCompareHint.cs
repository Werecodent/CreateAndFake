using CreateAndFake.Design;
using CreateAndFake.Design.Randomization;

namespace CreateAndFake.ValuerTool.CompareHints;

/// <summary>Handles comparing <see cref="SeededRandom"/> instances for <see cref="IValuer"/>.</summary>
public sealed class SeededRandomCompareHint : CompareHint<SeededRandom>
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        SeededRandom? expected,
        SeededRandom? actual,
        ValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return valuer.Compare(ExtractInternals(expected, valuer), ExtractInternals(actual, valuer));
    }

    /// <inheritdoc/>
    protected override int GetHashCode(SeededRandom? item, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return valuer.GetHashCode(ExtractInternals(item, valuer));
    }

    /// <summary></summary>
    /// <param name="item"></param>
    /// <param name="valuer"></param>
    /// <returns></returns>
    private static object? ExtractInternals(SeededRandom? item, ValuerChainer valuer)
    {
        if (item == null)
        {
            return null;
        }
        else if (valuer.Options.IgnoreCurrentRandomSeed)
        {
            return item.InitialSeed;
        }
        else
        {
            return new[] { item.InitialSeed, item.Seed };
        }
    }
}
