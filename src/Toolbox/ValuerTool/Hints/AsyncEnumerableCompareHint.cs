using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing <see cref="IAsyncEnumerable{T}"/> collections for <see cref="IValuer"/>.</summary>
public sealed class AsyncEnumerableCompareHint : CompareHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.AsyncEnumerableHint;

    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer valuer)
    {
        return expected.GetType().Inherits(typeof(IAsyncEnumerable<>))
            && actual.GetType().Inherits(typeof(IAsyncEnumerable<>));
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object expected,
        object actual,
        IValuerChainer valuer
    )
    {
        if (valuer.Options.SkipAsyncValues)
        {
            return [];
        }
        else
        {
            throw new EngineException(
                $"Cannot compare IAsyncEnumerables in synchronous context using {nameof(IValuer)}. "
                    + $"Use {nameof(IAsserter)} to compare IAsyncEnumerables in asynchronous context."
            );
        }
    }

    /// <inheritdoc/>
    protected override IAsyncEnumerable<Difference> CompareAsync(
        object expected,
        object actual,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        Type expectedType = TypeDescriber.FindConcreteInterface(
            expected.GetType(),
            typeof(IAsyncEnumerable<>)
        );
        Type actualType = TypeDescriber.FindConcreteInterface(
            actual.GetType(),
            typeof(IAsyncEnumerable<>)
        );

        if (expectedType != actualType)
        {
            return AsyncEnumHelper.CreateFrom([new Difference(expectedType, actualType)]);
        }

        return (IAsyncEnumerable<Difference>)
            GetType()
                .GetMethod(
                    nameof(CompareAsyncHandler),
                    BindingFlags.Static | BindingFlags.NonPublic
                )!
                .MakeGenericMethod(expectedType.GetGenericArguments().Single())
                .Invoke(null, [expected, actual, valuer, canceler])!;
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object item, IValuerChainer valuer)
    {
        if (valuer.Options.SkipAsyncValues)
        {
            return 0;
        }
        else
        {
            throw new EngineException(
                $"Cannot hash IAsyncEnumerable in synchronous context using {nameof(IValuer)}. "
                    + "Collect into a synchronous collection before attempting to hash."
            );
        }
    }

    /// <inheritdoc/>
    protected override Task<int> GetHashCodeAsync(
        object item,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        return (Task<int>)
            GetType()
                .GetMethod(
                    nameof(GetHashCodeHandler),
                    BindingFlags.Static | BindingFlags.NonPublic
                )!
                .MakeGenericMethod(item.GetType().GetGenericArguments().Single())
                .Invoke(null, [item, valuer, canceler])!;
    }

    /// <inheritdoc cref="Compare"/>
    /// <typeparam name="T">Item <see cref="Type"/> being compared.</typeparam>
    private static async IAsyncEnumerable<Difference> CompareAsyncHandler<T>(
        IAsyncEnumerable<T> expected,
        IAsyncEnumerable<T> actual,
        IValuerChainer valuer,
        [EnumeratorCancellation] CancellationToken canceler
    )
    {
        if (valuer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }

        IAsyncEnumerator<T> expectedEnumerator = expected.GetAsyncEnumerator(canceler);
        IAsyncEnumerator<T> actualEnumerator = actual.GetAsyncEnumerator(canceler);
        await using (expectedEnumerator.ConfigureAwait(false))
        await using (actualEnumerator.ConfigureAwait(false))
        {
            int index = 0;
            while (await expectedEnumerator.MoveNextAsync().ConfigureAwait(false))
            {
                if (await actualEnumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    await foreach (
                        Difference diff in valuer
                            .CompareAsync(expectedEnumerator.Current, actualEnumerator.Current)
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
                        new Difference(expectedEnumerator.Current, "'outofbounds'")
                    );
                }
                index++;
            }
            while (await actualEnumerator.MoveNextAsync().ConfigureAwait(false))
            {
                yield return new Difference(
                    index++,
                    new Difference("'outofbounds'", actualEnumerator.Current)
                );
            }
        }
    }

    /// <inheritdoc cref="GetHashCodeAsync"/>
    /// <typeparam name="T">Item <see cref="Type"/> being compared.</typeparam>
    private static async Task<int> GetHashCodeHandler<T>(
        IAsyncEnumerable<T> item,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        int hash = ValueComparer.BaseHash;
        await foreach (T current in item.WithCancellation(canceler).ConfigureAwait(false))
        {
            hash =
                hash * ValueComparer.HashMultiplier
                + await valuer.GetHashCodeAsync(current, canceler).ConfigureAwait(false);
        }
        return hash;
    }
}
