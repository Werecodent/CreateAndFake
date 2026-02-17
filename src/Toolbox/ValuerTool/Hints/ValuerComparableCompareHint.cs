using CreateAndFake.Design;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IValuerComparable"/> instances for <see cref="IValuer"/>.</summary>
public sealed class ValuerComparableCompareHint : CompareHint<IValuerComparable>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.ValuerComparableHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IValuerComparable expected,
        IValuerComparable actual,
        IValuerChainer valuer
    )
    {
        return expected.Compare(actual, valuer);
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IValuerComparable item, IValuerChainer valuer)
    {
        return item.GetValueHash(valuer);
    }
}
