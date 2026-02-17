using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IValueEquatable"/> instances for <see cref="IValuer"/>.</summary>
public sealed class ValueEquatableCompareHint : CompareHint<IValueEquatable>
{
    /// <summary>Compares equatables by value as well.</summary>
    private static readonly PrivateObjectCompareHint _NestedHint = new();

    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.ValueEquatableHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IValueEquatable expected,
        IValueEquatable actual,
        IValuerChainer valuer
    )
    {
        return LazyCompare(expected, actual, valuer);
    }

    /// <inheritdoc cref="Compare"/>
    private static IEnumerable<Difference> LazyCompare(
        IValueEquatable expected,
        IValueEquatable actual,
        IValuerChainer valuer
    )
    {
        if (!expected.ValuesEqual(actual))
        {
            yield return new Difference(
                $".{nameof(IValuerEquatable.ValuesEqual)}",
                new Difference(true, false)
            );

            DifferenceHintResult byValues = _NestedHint.TryCompare(expected, actual, valuer);
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
    protected override int GetHashCode(IValueEquatable item, IValuerChainer valuer)
    {
        return ValueComparer.Use.GetHashCode(item);
    }
}
