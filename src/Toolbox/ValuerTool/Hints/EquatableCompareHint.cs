using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IEquatable{T}"/> instances for <see cref="IValuer"/>.</summary>
public sealed class EquatableCompareHint : CompareHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.EquatableHint;

    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer chainer)
    {
        return chainer.Options.UseEquatableComparisons
            && expected
                .GetType()
                .Inherits(typeof(IEquatable<>).MakeGenericType(expected.GetType()));
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object expected,
        object actual,
        IValuerChainer chainer
    )
    {
        if (!expected.Equals(actual))
        {
            return [new Difference(expected, actual)];
        }
        else
        {
            return [];
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object item, IValuerChainer chainer)
    {
        return item.GetHashCode();
    }
}
