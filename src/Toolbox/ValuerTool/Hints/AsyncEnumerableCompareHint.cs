using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles the comparing of <see cref="IAsyncEnumerable{T}"/>s.</summary>
public sealed class AsyncEnumerableCompareHint : CompareHint
{
    /// <summary>Generic method for comparisons.</summary>
    private static readonly MethodInfo _CompareAsyncHandler =
        typeof(AsyncEnumerableCompareHint).GetMethod(
            nameof(CompareAsyncHandler),
            BindingFlags.Static | BindingFlags.NonPublic
        )!;

    /// <summary>Generic method for hashing.</summary>
    private static readonly MethodInfo _GetHashCodeHandler =
        typeof(AsyncEnumerableCompareHint).GetMethod(
            nameof(GetHashCodeHandler),
            BindingFlags.Static | BindingFlags.NonPublic
        )!;

    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.AsyncEnumerableHint;

    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer chainer)
    {
        return expected.GetType().Inherits(typeof(IAsyncEnumerable<>))
            && actual.GetType().Inherits(typeof(IAsyncEnumerable<>));
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object expected,
        object actual,
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
                Cannot compare {nameof(Type)}s of '{nameof(IAsyncEnumerable<>)}' in 
                synchronous context when {nameof(ValuerOptions.SkipAsyncValues)} is {false}. 
                Use an asynchronous method or override the setting.
                """
            );
        }
    }

    /// <inheritdoc/>
    protected override IAsyncEnumerable<Difference> CompareAsync(
        object expected,
        object actual,
        IValuerChainer chainer,
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
            _CompareAsyncHandler
                .MakeGenericMethod(expectedType.GetGenericArguments().Single())
                .Invoke(null, [expected, actual, chainer, canceler])!;
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object item, IValuerChainer chainer)
    {
        if (chainer.Options.SkipAsyncValues)
        {
            return 0;
        }
        else
        {
            throw new EngineException(
                $"""
                Cannot hash {nameof(Type)}s of '{nameof(IAsyncEnumerable<>)}' in 
                synchronous context when {nameof(ValuerOptions.SkipAsyncValues)} is {false}. 
                Use an asynchronous method or override the setting.
                """
            );
        }
    }

    /// <inheritdoc/>
    protected override Task<int> GetHashCodeAsync(
        object item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        Type itemType = TypeDescriber.FindConcreteInterface(
            item.GetType(),
            typeof(IAsyncEnumerable<>)
        );

        return (Task<int>)
            _GetHashCodeHandler
                .MakeGenericMethod(itemType.GetGenericArguments().Single())
                .Invoke(null, [item, chainer, canceler])!;
    }

    /// <inheritdoc cref="Compare"/>
    /// <typeparam name="T">The enumerable item <see cref="Type"/>.</typeparam>
    private static async IAsyncEnumerable<Difference> CompareAsyncHandler<T>(
        IAsyncEnumerable<T> expected,
        IAsyncEnumerable<T> actual,
        IValuerChainer chainer,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        canceler.ThrowIfCancellationRequested();
        if (chainer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
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
                canceler.ThrowIfCancellationRequested();

                if (await actualEnumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    canceler.ThrowIfCancellationRequested();

                    await foreach (
                        Difference diff in chainer
                            .CompareAsync(
                                expectedEnumerator.Current,
                                actualEnumerator.Current,
                                canceler
                            )
                            .ConfigureAwait(false)
                    )
                    {
                        yield return new Difference(index, diff);
                        canceler.ThrowIfCancellationRequested();
                    }
                }
                else
                {
                    yield return new Difference(
                        index,
                        new Difference(expectedEnumerator.Current, "'outofbounds'")
                    );
                    canceler.ThrowIfCancellationRequested();
                }
                index++;
            }

            canceler.ThrowIfCancellationRequested();

            while (await actualEnumerator.MoveNextAsync().ConfigureAwait(false))
            {
                yield return new Difference(
                    index++,
                    new Difference("'outofbounds'", actualEnumerator.Current)
                );
                canceler.ThrowIfCancellationRequested();
            }

            canceler.ThrowIfCancellationRequested();
        }
    }

    /// <inheritdoc cref="GetHashCodeAsync"/>
    /// <typeparam name="T">The enumerable item <see cref="Type"/>.</typeparam>
    private static async Task<int> GetHashCodeHandler<T>(
        IAsyncEnumerable<T> item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        canceler.ThrowIfCancellationRequested();

        int hash = ValueComparer.BaseHash;
        await foreach (T current in item.WithCancellation(canceler).ConfigureAwait(false))
        {
            canceler.ThrowIfCancellationRequested();
            hash =
                hash * ValueComparer.HashMultiplier
                + await chainer.GetHashCodeAsync(current, canceler).ConfigureAwait(false);
        }

        canceler.ThrowIfCancellationRequested();
        return hash;
    }
}
