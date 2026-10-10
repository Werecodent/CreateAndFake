using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Hints;

/// <inheritdoc/>
public sealed class FakedCompareHint : CompareHint<IFaked>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.FakedHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IFaked expected,
        IFaked actual,
        IValuerChainer chainer
    )
    {
        return chainer.Compare(expected.FakeMeta, actual.FakeMeta);
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IFaked item, IValuerChainer chainer)
    {
        return chainer.GetHashCode(item.FakeMeta);
    }
}
