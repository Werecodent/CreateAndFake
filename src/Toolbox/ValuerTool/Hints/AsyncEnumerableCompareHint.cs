using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Hints;

/// <summary>Handles the comparing of <see cref="IAsyncEnumerable{T}"/>s.</summary>
public sealed class AsyncEnumerableCompareHint : CompareHint
{
    /// <summary>Generic method to convert synchronous collections to asynchronous.</summary>
    private static readonly MethodInfo _AsyncConverter = typeof(AsyncEnumHelper).GetMethod(
        nameof(AsyncEnumHelper.CreateFrom)
    )!;

    /// <summary>Generic method for comparisons.</summary>
    private static readonly MethodInfo _CompareAsyncHandler =
        typeof(AsyncEnumerableCompareHint).GetMethod(
            nameof(ContentsCompareTimedAsync),
            BindingFlags.Static | BindingFlags.NonPublic
        )!;

    /// <summary>Generic method for hashing.</summary>
    private static readonly MethodInfo _GetHashCodeHandler =
        typeof(AsyncEnumerableCompareHint).GetMethod(
            nameof(ContentsGetHashCodeTimedAsync),
            BindingFlags.Static | BindingFlags.NonPublic
        )!;

    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.AsyncEnumerableHint;

    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer chainer)
    {
        Type expectedType = expected.GetType();
        Type actualType = actual.GetType();

        if (expectedType.Inherits(typeof(IAsyncEnumerable<>)))
        {
            return actualType.Inherits(typeof(IAsyncEnumerable<>))
                || actualType.Inherits(typeof(IEnumerable<>));
        }
        else
        {
            return actualType.Inherits(typeof(IAsyncEnumerable<>))
                && expectedType.Inherits(typeof(IEnumerable<>));
        }
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
        object convertedExpected = ConvertFromSync(expected);
        object convertedActual = ConvertFromSync(actual);

        Type expectedType = TypeDescriber.FindConcreteType(
            convertedExpected.GetType(),
            typeof(IAsyncEnumerable<>)
        );
        Type actualType = TypeDescriber.FindConcreteType(
            convertedActual.GetType(),
            typeof(IAsyncEnumerable<>)
        );

        if (expectedType != actualType)
        {
            return AsyncEnumHelper.CreateFrom([
                new Difference(expected.GetType(), actual.GetType()),
            ]);
        }

        return (IAsyncEnumerable<Difference>)
            _CompareAsyncHandler
                .MakeGenericMethod(expectedType.GetGenericArguments().Single())
                .Invoke(null, [convertedExpected, convertedActual, chainer, canceler])!;
    }

    /// <summary>Converts <paramref name="collection"/> to asynchronous if not already.</summary>
    /// <param name="collection">Series to potentially convert.</param>
    /// <returns>The asynchronous result.</returns>
    private static object ConvertFromSync(object collection)
    {
        Type collectionType = collection.GetType();
        if (collectionType.Inherits(typeof(IAsyncEnumerable<>)))
        {
            return collection;
        }
        else
        {
            Type contentType = TypeDescriber
                .FindConcreteType(collectionType, typeof(IEnumerable<>))
                .GetGenericArguments()[0];
            return _AsyncConverter.MakeGenericMethod([contentType]).Invoke(null, [collection])!;
        }
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
        object convertedItem = ConvertFromSync(item);
        Type itemType = TypeDescriber.FindConcreteType(
            convertedItem.GetType(),
            typeof(IAsyncEnumerable<>)
        );

        return (Task<int>)
            _GetHashCodeHandler
                .MakeGenericMethod(itemType.GetGenericArguments().Single())
                .Invoke(null, [convertedItem, chainer, canceler])!;
    }

    /// <inheritdoc cref="ContentsCompareAsync"/>
    private static async IAsyncEnumerable<Difference> ContentsCompareTimedAsync<T>(
        IAsyncEnumerable<T> expected,
        IAsyncEnumerable<T> actual,
        IValuerChainer chainer,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        using CancellationTokenSource timeoutSource =
            CancellationTokenSource.CreateLinkedTokenSource(canceler);

        timeoutSource.CancelAfter(chainer.Options.AsyncTimeout);

        await foreach (
            Difference diff in ContentsCompareAsync(expected, actual, chainer, timeoutSource.Token)
                .ConfigureAwait(false)
        )
        {
            yield return diff;
        }
    }

    /// <inheritdoc cref="Compare"/>
    /// <typeparam name="T">The enumerable item <see cref="Type"/>.</typeparam>
    private static async IAsyncEnumerable<Difference> ContentsCompareAsync<T>(
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

    /// <inheritdoc cref="ContentsGetHashCodeAsync"/>
    private static async Task<int> ContentsGetHashCodeTimedAsync<T>(
        IAsyncEnumerable<T> item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        Task<int> hasher = ContentsGetHashCodeAsync(item, chainer, canceler);
        if (
            await Task.WhenAny(hasher, Task.Delay(chainer.Options.AsyncTimeout, canceler))
                .ConfigureAwait(false) == hasher
        )
        {
            return await hasher.ConfigureAwait(false);
        }
        else
        {
            canceler.ThrowIfCancellationRequested();
            throw new EngineException(
                $"""
                Attempting to iterate the {TypeDescriber.ExpandedName(item.GetType())} exceeded the 
                timeout ({nameof(ValuerOptions.AsyncTimeout)}) of '{chainer.Options.AsyncTimeout}'.
                """
            );
        }
    }

    /// <inheritdoc cref="GetHashCodeAsync"/>
    /// <typeparam name="T">The enumerable item <see cref="Type"/>.</typeparam>
    private static async Task<int> ContentsGetHashCodeAsync<T>(
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
