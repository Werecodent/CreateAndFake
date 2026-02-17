using CreateAndFake.AsserterTool;
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
        IValuerAsyncComparable expected,
        IValuerAsyncComparable actual,
        IValuerChainer chainer
    )
    {
        if (chainer.Options.SkipAsyncValues)
        {
            return [];
        }
        else
        {
            throw new EngineException(
                $"""
                Cannot compare IValuerAsyncComparables in synchronous context 
                using {nameof(IValuer)}. Use {nameof(IAsserter)} to compare 
                IAsyncEnumerables in asynchronous context.
                """
            );
        }
    }

    /// <inheritdoc/>
    protected override IAsyncEnumerable<Difference> CompareAsync(
        IValuerAsyncComparable expected,
        IValuerAsyncComparable actual,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        return expected.CompareAsync(actual, chainer, canceler);
    }

    /// <inheritdoc/>
    protected override int GetHashCode(IValuerAsyncComparable item, IValuerChainer chainer)
    {
        if (chainer.Options.SkipAsyncValues)
        {
            return 0;
        }
        else
        {
            throw new EngineException(
                $"""
                Cannot hash IValuerAsyncComparable in synchronous context using {nameof(IValuer)}. 
                Collect into a synchronous collection before attempting to hash.
                """
            );
        }
    }

    /// <inheritdoc/>
    protected override Task<int> GetHashCodeAsync(
        IValuerAsyncComparable item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        return item.GetValueHashAsync(chainer, canceler);
    }
}
