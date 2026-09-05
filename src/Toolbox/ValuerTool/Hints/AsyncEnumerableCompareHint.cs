using System.Runtime.CompilerServices;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Hints;

/// <summary>Handles the comparing of <see cref="IAsyncEnumerable{T}"/>s.</summary>
public sealed class AsyncEnumerableCompareHint : CompareHint
{
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
                $"Cannot compare {nameof(Type)}s of '{nameof(IAsyncEnumerable<>)}' in "
                    + $"synchronous context when {nameof(ValuerOptions.SkipAsyncValues)} "
                    + $"is {false}. Use an asynchronous method or override the setting."
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
        dynamic convertedExpected = ConvertFromSync(expected, chainer, canceler);
        dynamic convertedActual = ConvertFromSync(actual, chainer, canceler);

        Type expectedType = GenericConverter.FindConcreteType(
            convertedExpected.GetType(),
            typeof(IAsyncEnumerable<>)
        );
        Type actualType = GenericConverter.FindConcreteType(
            convertedActual.GetType(),
            typeof(IAsyncEnumerable<>)
        );

        if (expectedType != actualType)
        {
            return AsyncSeriesHelper.CreateFromAsync(
                [new Difference(expected.GetType(), actual.GetType())],
                chainer.Options.IterationLimit,
                canceler
            );
        }

        return ContentsCompareAsync(convertedExpected, convertedActual, chainer, canceler);
    }

    /// <summary>Converts <paramref name="collection"/> to asynchronous if not already.</summary>
    /// <param name="collection">Series to potentially convert.</param>
    /// <param name="chainer">Handles comparing child values.</param>
    /// <param name="canceler">Aborts execution if triggered</param>
    /// <returns>The asynchronous result.</returns>
    private static dynamic ConvertFromSync(
        object collection,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        if (collection.GetType().Inherits(typeof(IAsyncEnumerable<>)))
        {
            return collection;
        }
        else
        {
            return AsyncSeriesHelper.CreateFromAsync(
                (dynamic)collection,
                chainer.Options.IterationLimit,
                canceler
            );
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
                $"Cannot hash {nameof(Type)}s of '{nameof(IAsyncEnumerable<>)}' in "
                    + $"synchronous context when {nameof(ValuerOptions.SkipAsyncValues)} "
                    + $"is {false}. Use an asynchronous method or override the setting."
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
        return ContentsGetHashCodeAsync(
            ConvertFromSync(item, chainer, canceler),
            chainer,
            canceler
        );
    }

    /// <inheritdoc cref="Compare"/>
    /// <typeparam name="T">The enumerable's item <see cref="Type"/>.</typeparam>
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
            while (
                await expectedEnumerator.MoveNextAsync().ConfigureAwait(false)
                && index < chainer.Options.IterationLimit
            )
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
                        new Difference(expectedEnumerator.Current, "'out of range'")
                    );
                    canceler.ThrowIfCancellationRequested();
                }
                index++;
            }

            canceler.ThrowIfCancellationRequested();

            while (
                await actualEnumerator.MoveNextAsync().ConfigureAwait(false)
                && index < chainer.Options.IterationLimit
            )
            {
                yield return new Difference(
                    index++,
                    new Difference("'out of range'", actualEnumerator.Current)
                );
                canceler.ThrowIfCancellationRequested();
            }

            canceler.ThrowIfCancellationRequested();

            ArgumentGuard.ThrowUponIterationLimit(index, chainer.Options.IterationLimit);
        }
    }

    /// <inheritdoc cref="GetHashCodeAsync"/>
    /// <typeparam name="T">The enumerable's item <see cref="Type"/>.</typeparam>
    private static async Task<int> ContentsGetHashCodeAsync<T>(
        IAsyncEnumerable<T> item,
        IValuerChainer chainer,
        CancellationToken canceler
    )
    {
        int hash = ValueComparer.BaseHash;
        await AsyncSeriesHelper
            .ForEachAsync(
                item,
                chainer.Options.IterationLimit,
                canceler,
                async current =>
                {
                    hash *= ValueComparer.HashMultiplier;
                    hash += await chainer.GetHashCodeAsync(current, canceler).ConfigureAwait(false);
                }
            )
            .ConfigureAwait(false);

        return hash;
    }
}
