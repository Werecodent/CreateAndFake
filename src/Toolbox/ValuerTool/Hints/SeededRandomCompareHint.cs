using CreateAndFake.Design;
using CreateAndFake.Design.Randomization;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="SeededRandom"/> instances for <see cref="IValuer"/>.</summary>
public sealed class SeededRandomCompareHint : CompareHint<SeededRandom>
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        SeededRandom? expected,
        SeededRandom? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return valuer.Compare(ExtractInternals(expected, valuer), ExtractInternals(actual, valuer));
    }

    /// <inheritdoc/>
    protected override int GetHashCode(SeededRandom? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return valuer.GetHashCode(ExtractInternals(item, valuer));
    }

    /// <summary>Test</summary>
    /// <param name="item"></param>
    /// <param name="valuer"></param>
    /// <returns></returns>
    private static object? ExtractInternals(SeededRandom? item, IValuerChainer valuer)
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
