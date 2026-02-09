using CreateAndFake.AsserterTool;
using CreateAndFake.Design;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IValuerEquatable"/> instances for <see cref="IValuer"/>.</summary>
public sealed class ValuerAsyncComparableCompareHint : CompareHint<IValuerAsyncComparable>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.ValuerAsyncComparableHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        IValuerAsyncComparable? expected,
        IValuerAsyncComparable? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer);

        if (valuer.Options.SkipAsyncValues)
        {
            return [];
        }
        else
        {
            throw new EngineException(
                $"Cannot compare IValuerAsyncComparables in synchronous context using {nameof(IValuer)}. "
                    + $"Use {nameof(IAsserter)} to compare IAsyncEnumerables in asynchronous context."
            );
        }
    }

    /// <inheritdoc/>
    protected override IAsyncEnumerable<Difference> CompareAsync(
        IValuerAsyncComparable? expected,
        IValuerAsyncComparable? actual,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(valuer, expected);

        return expected.CompareAsync(actual, valuer, canceler);
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IValuerAsyncComparable? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer);

        if (valuer.Options.SkipAsyncValues)
        {
            return 0;
        }
        else
        {
            throw new EngineException(
                $"Cannot hash IValuerAsyncComparable in synchronous context using {nameof(IValuer)}. "
                    + "Collect into a synchronous collection before attempting to hash."
            );
        }
    }

    /// <inheritdoc/>
    protected override Task<int> GetHashCodeAsync(
        IValuerAsyncComparable? item,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(item, valuer);

        return item.GetValueHashAsync(valuer, canceler);
    }
}
