using CreateAndFake.Design.Types;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing stateless objects for <see cref="IValuer"/>.</summary>
public sealed class StatelessCompareHint : CompareHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.StatelessHint;

    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer chainer)
    {
        Type type = expected.GetType();
        return !InheritanceTracker.For(type).AllProperties.Any(p => p.CanRead)
            && !InheritanceTracker.For(type).AllFields.Any();
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object expected,
        object actual,
        IValuerChainer chainer
    )
    {
        return [];
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object item, IValuerChainer chainer)
    {
        return item.GetType().GetHashCode();
    }
}
