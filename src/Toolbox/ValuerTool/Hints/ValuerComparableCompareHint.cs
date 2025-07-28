using CreateAndFake.Design;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IValuerComparable"/> instances for <see cref="IValuer"/>.</summary>
public sealed class ValuerComparableCompareHint : CompareHint<IValuerComparable>
{
    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IValuerComparable? expected,
        IValuerComparable? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));
        ArgumentGuard.ThrowIfNull(expected, nameof(expected));

        return expected.Compare(actual, valuer);
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IValuerComparable? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item, nameof(item));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return item.GetValueHash(valuer);
    }
}
