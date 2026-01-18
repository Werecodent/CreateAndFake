using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IValuerEquatable"/> instances for <see cref="IValuer"/>.</summary>
public sealed class ValuerEquatableCompareHint : CompareHint<IValuerEquatable>
{
    /// <summary>Compares equatables by value as well.</summary>
    private static readonly ObjectCompareHint _NestedHint = new(
        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
    );

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IValuerEquatable? expected,
        IValuerEquatable? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer);

        return LazyCompare(expected, actual, valuer);
    }

    /// <inheritdoc cref="Compare"/>
    private static IEnumerable<Difference> LazyCompare(
        IValuerEquatable? expected,
        IValuerEquatable? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(expected);

        if (!expected.ValuesEqual(actual, valuer))
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
    protected override int GetHashCode(IValuerEquatable? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item, valuer);

        return item.GetValueHash(valuer);
    }
}
