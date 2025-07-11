using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IAsyncEnumerable{T}"/> collections for <see cref="IValuer"/>.</summary>
public sealed class AsyncEnumerableCompareHint : CompareHint
{
    /// <inheritdoc/>
    protected override bool Supports(object? expected, object? actual, IValuerChainer valuer)
    {
        return (expected?.GetType()).Inherits(typeof(IAsyncEnumerable<>))
            && (actual?.GetType()).Inherits(typeof(IAsyncEnumerable<>));
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

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
    protected override Task<IEnumerable<Difference>> CompareAsync(
        object? expected,
        object? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        if (expected == null || actual == null)
        {
            return Task.FromResult<IEnumerable<Difference>>([new Difference(expected, actual)]);
        }

        Type expectedType = TypeDescriber
            .For(expected.GetType())
            .FindConcreteInterface(typeof(IAsyncEnumerable<>));
        Type actualType = TypeDescriber
            .For(actual.GetType())
            .FindConcreteInterface(typeof(IAsyncEnumerable<>));

        if (expectedType != actualType)
        {
            return Task.FromResult<IEnumerable<Difference>>(
                [new Difference(expectedType, actualType)]
            );
        }

        return (Task<IEnumerable<Difference>>)
            GetType()
                .GetMethod(
                    nameof(CompareAsyncHandler),
                    BindingFlags.Static | BindingFlags.NonPublic
                )!
                .MakeGenericMethod(expectedType.GetGenericArguments().Single())
                .Invoke(null, [expected, actual, valuer])!;
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        if (valuer.Options.SkipAsyncValues)
        {
            return 0;
        }
        else
        {
            throw new ToolException(
                $"Cannot hash IAsyncEnumerable in synchronous context using {nameof(IValuer)}. "
                    + "Collect into a synchronous collection before attempting to hash."
            );
        }
    }

    /// <inheritdoc/>
    protected override Task<int> GetHashCodeAsync(object? item, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item, nameof(item));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return (Task<int>)
            GetType()
                .GetMethod(
                    nameof(GetHashCodeHandler),
                    BindingFlags.Static | BindingFlags.NonPublic
                )!
                .MakeGenericMethod(item.GetType().GetGenericArguments().Single())
                .Invoke(null, [item, valuer])!;
    }

    /// <inheritdoc cref="Compare"/>
    /// <typeparam name="T">Item <see cref="Type"/> being compared.</typeparam>
    private static async Task<IEnumerable<Difference>> CompareAsyncHandler<T>(
        IAsyncEnumerable<T> expected,
        IAsyncEnumerable<T> actual,
        IValuerChainer valuer
    )
    {
        List<Difference> differences = [];

        if (valuer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            differences.Add(new Difference(expected.GetType(), actual.GetType()));
        }

        IAsyncEnumerator<T> expectedEnumerator = expected.GetAsyncEnumerator();
        IAsyncEnumerator<T> actualEnumerator = actual.GetAsyncEnumerator();
        await using (expectedEnumerator.ConfigureAwait(false))
        await using (actualEnumerator.ConfigureAwait(false))
        {
            int index = 0;
            while (await expectedEnumerator.MoveNextAsync().ConfigureAwait(false))
            {
                if (await actualEnumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    differences.AddRange(
                        (
                            await valuer
                                .CompareAsync(expectedEnumerator.Current, actualEnumerator.Current)
                                .ConfigureAwait(false)
                        ).Select(diff => new Difference(index, diff))
                    );
                }
                else
                {
                    differences.Add(
                        new Difference(
                            index,
                            new Difference(expectedEnumerator.Current, "'outofbounds'")
                        )
                    );
                }
                index++;
            }
            while (await actualEnumerator.MoveNextAsync().ConfigureAwait(false))
            {
                differences.Add(
                    new Difference(
                        index++,
                        new Difference("'outofbounds'", actualEnumerator.Current)
                    )
                );
            }
        }

        return differences;
    }

    /// <inheritdoc cref="GetHashCodeAsync"/>
    /// <typeparam name="T">Item <see cref="Type"/> being compared.</typeparam>
    private static async Task<int> GetHashCodeHandler<T>(
        IAsyncEnumerable<T> item,
        IValuerChainer valuer
    )
    {
        int hash = ValueComparer.BaseHash;
        await foreach (T current in item.ConfigureAwait(false))
        {
            hash =
                hash * ValueComparer.HashMultiplier
                + await valuer.GetHashCodeAsync(current).ConfigureAwait(false);
        }
        return hash;
    }
}
