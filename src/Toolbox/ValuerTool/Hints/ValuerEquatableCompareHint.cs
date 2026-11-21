using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IValuerEquatable"/> instances for <see cref="IValuer"/>.</summary>
public sealed class ValuerEquatableCompareHint : CompareHint<IValuerEquatable>
{
    /// <summary>Compares equatables by value as well.</summary>
    private static readonly PrivateObjectCompareHint _NestedHint = new();

    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.ValuerEquatableHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IValuerEquatable expected,
        IValuerEquatable actual,
        IValuerChainer chainer
    )
    {
        return LazyCompare(expected, actual, chainer);
    }

    /// <inheritdoc cref="Compare"/>
    private static IEnumerable<Difference> LazyCompare(
        IValuerEquatable expected,
        IValuerEquatable actual,
        IValuerChainer chainer
    )
    {
        if (!expected.ValuesEqual(actual, chainer))
        {
            yield return new Difference(
                $".{nameof(IValuerEquatable.ValuesEqual)}",
                new Difference(true, false)
            );

            DifferenceHintResult byValues = _NestedHint.TryToCompare(expected, actual, chainer);
            if (byValues.HasData)
            {
                foreach (Difference difference in byValues.Data!)
                {
                    yield return difference;
                }
            }
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IValuerEquatable item, IValuerChainer chainer)
    {
        return item.GetValueHash(chainer);
    }
}
