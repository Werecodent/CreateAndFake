using System.Collections;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <inheritdoc/>
public sealed class StructuralEquatableCompareHint : CompareHint<IStructuralEquatable>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.StructuralEquatableHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IStructuralEquatable expected,
        IStructuralEquatable actual,
        IValuerChainer chainer
    )
    {
        if (!expected.Equals(actual, chainer))
        {
            return [new Difference(expected, actual)];
        }
        else
        {
            return [];
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IStructuralEquatable item, IValuerChainer chainer)
    {
        return item.GetHashCode(chainer);
    }
}
