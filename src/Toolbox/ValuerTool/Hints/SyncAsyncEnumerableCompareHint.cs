using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.AsserterTool;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IAsyncEnumerable{T}"/> with <see cref="IEnumerable{T}"/> collections for <see cref="IValuer"/>.</summary>
public sealed class SyncAsyncEnumerableCompareHint : CompareHint
{
    /// <inheritdoc/>
    protected override bool Supports(object? expected, object? actual, IValuerChainer valuer)
    {
        return (expected?.GetType()).Inherits(typeof(IEnumerable<>))
                && (actual?.GetType()).Inherits(typeof(IAsyncEnumerable<>))
            || (expected?.GetType()).Inherits(typeof(IAsyncEnumerable<>))
                && (actual?.GetType()).Inherits(typeof(IEnumerable<>));
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
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
            throw new ToolException(
                $"Cannot compare IAsyncEnumerables in synchronous context using {nameof(IValuer)}. "
                    + $"Use {nameof(IAsserter)} to compare IAsyncEnumerables in asynchronous context."
            );
        }
    }

    /// <inheritdoc/>
    protected override IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(valuer);

        if (expected == null || actual == null)
        {
            return AsyncEnumHelper.CreateFrom([new Difference(expected, actual)]);
        }

        if (expected.GetType().Inherits(typeof(IAsyncEnumerable<>)))
        {
            return CompareAsync(expected, actual, true, valuer, canceler);
        }
        else
        {
            return CompareAsync(actual, expected, false, valuer, canceler);
        }
    }

    private IAsyncEnumerable<Difference> CompareAsync(
        object asyncSeries,
        object syncSeries,
        bool isExpectedFirst,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(valuer);

        Type asyncType = TypeDescriber.FindConcreteInterface(
            asyncSeries.GetType(),
            typeof(IAsyncEnumerable<>)
        );
        Type syncType = TypeDescriber.FindConcreteInterface(
            syncSeries.GetType(),
            typeof(IEnumerable<>)
        );

        string asyncGeneric = $"<{asyncType.GetGenericArguments()[0]}>";
        string syncGeneric = $"<{syncType.GetGenericArguments()[0]}>";
        if (asyncGeneric != syncGeneric)
        {
            return AsyncEnumHelper.CreateFrom([
                CreateDiff(asyncGeneric, syncGeneric, isExpectedFirst),
            ]);
        }

        return (IAsyncEnumerable<Difference>)
            GetType()
                .GetMethod(
                    nameof(CompareAsyncHandler),
                    BindingFlags.Static | BindingFlags.NonPublic
                )!
                .MakeGenericMethod(asyncType.GetGenericArguments().Single())
                .Invoke(null, [asyncSeries, syncSeries, isExpectedFirst, valuer, canceler])!;
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object? item, IValuerChainer valuer)
    {
        return 0;
    }

    /// <inheritdoc/>
    protected override Task<int> GetHashCodeAsync(
        object? item,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        return Task.FromResult(0);
    }

    /// <inheritdoc cref="Compare"/>
    /// <typeparam name="T">Item <see cref="Type"/> being compared.</typeparam>
    private static async IAsyncEnumerable<Difference> CompareAsyncHandler<T>(
        IAsyncEnumerable<T> asyncSeries,
        IEnumerable<T> syncSeries,
        bool isExpectedFirst,
        IValuerChainer valuer,
        [EnumeratorCancellation] CancellationToken canceler
    )
    {
        if (valuer.Options.CheckCollectionType)
        {
            yield return CreateDiff(asyncSeries.GetType(), syncSeries.GetType(), isExpectedFirst);
        }

        IEnumerator<T> syncEnumerator = syncSeries.GetEnumerator();
        IAsyncEnumerator<T> asyncEnumerator = asyncSeries.GetAsyncEnumerator(canceler);
        await using (asyncEnumerator.ConfigureAwait(false))
        {
            int index = 0;
            while (await asyncEnumerator.MoveNextAsync().ConfigureAwait(false))
            {
                if (syncEnumerator.MoveNext())
                {
                    await foreach (
                        Difference diff in InnerCompareAsync(
                                asyncEnumerator.Current,
                                syncEnumerator.Current,
                                isExpectedFirst,
                                valuer
                            )
                            .WithCancellation(canceler)
                            .ConfigureAwait(false)
                    )
                    {
                        yield return new Difference(index, diff);
                    }
                    canceler.ThrowIfCancellationRequested();
                }
                else
                {
                    yield return new Difference(
                        index,
                        CreateDiff(asyncEnumerator.Current, "'outofbounds'", isExpectedFirst)
                    );
                }
                index++;
            }
            while (syncEnumerator.MoveNext())
            {
                yield return new Difference(
                    index++,
                    CreateDiff("'outofbounds'", syncEnumerator.Current, isExpectedFirst)
                );
            }
        }
    }

    private static IAsyncEnumerable<Difference> InnerCompareAsync(
        object? firstValue,
        object? secondValue,
        bool isExpectedFirst,
        IValuerChainer valuer
    )
    {
        return isExpectedFirst
            ? valuer.CompareAsync(firstValue, secondValue)
            : valuer.CompareAsync(secondValue, firstValue);
    }

    private static Difference CreateDiff(
        object? firstValue,
        object? secondValue,
        bool isExpectedFirst
    )
    {
        return isExpectedFirst
            ? new Difference(firstValue, secondValue)
            : new Difference(secondValue, firstValue);
    }
}
