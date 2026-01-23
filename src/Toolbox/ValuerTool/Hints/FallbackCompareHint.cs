using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing instances needing to use regular equality/hashing for <see cref="IValuer"/>.</summary>
public sealed class FallbackCompareHint : CompareHint
{
    /// <summary>Compares by value if regular equality fails for expanded details.</summary>
    private static readonly ObjectCompareHint _NestedHint = new(false);

    /// <inheritdoc/>
    protected override bool Supports(object? expected, object? actual, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer);

        Type? type = (expected ?? actual)?.GetType();

        return type != null && valuer.Options.FallbackTypes.Contains(type);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        IValuerChainer valuer
    )
    {
        if (expected != actual)
        {
            yield return new Difference(".equals", new Difference(expected, actual));

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
    protected override async IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        IValuerChainer valuer,
        [EnumeratorCancellation] CancellationToken canceler
    )
    {
        if (expected != actual)
        {
            yield return new Difference(".equals", new Difference(expected, actual));

            DifferenceHintAsyncResult byValues = _NestedHint.TryAsyncCompare(
                expected,
                actual,
                valuer
            );
            if (byValues.HasData)
            {
                await foreach (
                    Difference diff in byValues
                        .Data!.WithCancellation(canceler)
                        .ConfigureAwait(false)
                )
                {
                    yield return diff;
                }
            }
        }
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object? item, IValuerChainer valuer)
    {
        return item?.GetHashCode() ?? ValueComparer.NullHash;
    }
}
