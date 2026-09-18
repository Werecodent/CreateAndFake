using System.Reflection;
using System.Runtime.CompilerServices;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Hints;

/// <summary>Handles the comparing of <see cref="IAsyncSet{T}"/>s.</summary>
public sealed class AsyncSetCompareHint : CompareHint
{
    /// <summary>Creator for <see cref="IAsyncEqualityComparer{T}"/> instances.</summary>
    private static readonly MethodInfo _ComparerMaker = typeof(IValuer).GetMethod(
        nameof(IValuer.ToAsyncComparer)
    )!;

    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.Disabled;

    /// <inheritdoc/>
    protected override bool Supports(object expected, object actual, IValuerChainer chainer)
    {
        Type expectedType = expected.GetType();
        Type actualType = actual.GetType();

        if (expectedType.Inherits(typeof(IAsyncSet<>)))
        {
            return actualType.Inherits(typeof(IAsyncSet<>)) || actualType.Inherits(typeof(ISet<>));
        }
        else
        {
            return actualType.Inherits(typeof(IAsyncSet<>))
                && expectedType.Inherits(typeof(ISet<>));
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
                $"Cannot compare {nameof(Type)}s of '{nameof(IAsyncSet<>)}' in "
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
            typeof(IAsyncSet<>)
        );
        Type actualType = GenericConverter.FindConcreteType(
            convertedActual.GetType(),
            typeof(IAsyncSet<>)
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
        Type type = collection.GetType();

        if (type.Inherits(typeof(IAsyncSet<>)))
        {
            return collection;
        }
        else
        {
            return AsyncHashSet.CreateFromAsync(
                (dynamic)collection,
                _ComparerMaker
                    .MakeGenericMethod(type.GetGenericArguments().Single())
                    .Invoke(chainer, []),
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
                $"Cannot hash {nameof(Type)}s of '{nameof(IAsyncSet<>)}' in "
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
        IAsyncSet<T> expected,
        IAsyncSet<T> actual,
        IValuerChainer chainer,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        canceler.ThrowIfCancellationRequested();
        if (chainer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            yield return new Difference(expected.GetType(), actual.GetType());
        }

        await foreach (
            T item in expected.FindMissingFromAsync(actual, canceler).ConfigureAwait(false)
        )
        {
            yield return new Difference(item, "'missing'");
        }

        await foreach (
            T item in actual.FindMissingFromAsync(expected, canceler).ConfigureAwait(false)
        )
        {
            yield return new Difference("'missing'", item);
        }
    }

    /// <inheritdoc cref="GetHashCodeAsync"/>
    /// <typeparam name="T">The enumerable's item <see cref="Type"/>.</typeparam>
    private static async Task<int> ContentsGetHashCodeAsync<T>(
        IAsyncSet<T> item,
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
