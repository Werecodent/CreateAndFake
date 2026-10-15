using Werecodent.CreateAndFake.Design.Comparisons;

namespace Werecodent.CreateAndFake.Design.Content;

/// <inheritdoc cref="IAsyncSet{T}"/>
public static class AsyncHashSet
{
    /// <summary>Creates a set with initial <paramref name="contents"/>.</summary>
    /// <typeparam name="T">Item content type.</typeparam>
    /// <param name="contents">Initial contents for the set.</param>
    /// <param name="comparer"><inheritdoc cref="AsyncHashSet{T}.Comparer" path="/summary"/></param>
    /// <param name="iterationLimit">Max number of items to iterate before throwing.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>The created set.</returns>
    public static AsyncHashSet<T> CreateFromAsync<T>(
        IAsyncEnumerable<T> contents,
        IAsyncEqualityComparer<T> comparer,
        int iterationLimit,
        CancellationToken canceler
    )
    {
        async Task<IDictionary<int, IList<T>>> setInitialContentsAsync()
        {
            Dictionary<int, IList<T>> results = [];

            await AsyncSeriesHelper
                .ForEachAsync(
                    contents,
                    iterationLimit,
                    canceler,
                    async item =>
                        _ = await AsyncHashSet<T>
                            .AddToAsync(results, item, comparer, canceler)
                            .ConfigureAwait(false)
                )
                .ConfigureAwait(false);

            return results;
        }

        return new AsyncHashSet<T>(setInitialContentsAsync(), comparer);
    }

    /// <inheritdoc cref="CreateFromAsync{T}(IAsyncEnumerable{T},IAsyncEqualityComparer{T},int,CancellationToken)"/>
    public static AsyncHashSet<T> CreateFromAsync<T>(
        IEnumerable<T> contents,
        IAsyncEqualityComparer<T> comparer,
        int iterationLimit,
        CancellationToken canceler
    )
    {
        return CreateFromAsync(
            AsyncSeriesHelper.CreateFromAsync(contents, iterationLimit),
            comparer,
            iterationLimit,
            canceler
        );
    }

    /// <inheritdoc cref="CreateFromAsync{T}(IAsyncEnumerable{T},IAsyncEqualityComparer{T},int,CancellationToken)"/>
    public static AsyncHashSet<T> CreateFromAsync<T>(
        IAsyncEnumerable<KeyValuePair<int, T>> contents,
        IAsyncEqualityComparer<T> comparer,
        int iterationLimit,
        CancellationToken canceler
    )
    {
        async Task<IDictionary<int, IList<T>>> setInitialContentsAsync()
        {
            Dictionary<int, IList<T>> results = [];

            await AsyncSeriesHelper
                .ForEachAsync(
                    contents,
                    iterationLimit,
                    canceler,
                    pair =>
                    {
                        if (results.TryGetValue(pair.Key, out IList<T>? list))
                        {
                            list.Add(pair.Value);
                        }
                        else
                        {
                            results.Add(pair.Key, [pair.Value]);
                        }
                    }
                )
                .ConfigureAwait(false);

            return results;
        }

        return new AsyncHashSet<T>(setInitialContentsAsync(), comparer);
    }

    /// <summary>
    ///     Uses <paramref name="factory"/> to generate items until the resulting hash size is <paramref name="size"/>.
    /// </summary>
    /// <param name="size">Size of collection to create.</param>
    /// <param name="factory">Method used to generate content.</param>
    /// <inheritdoc cref="CreateFromAsync{T}(IAsyncEnumerable{T},IAsyncEqualityComparer{T},int,CancellationToken)"/>
    internal static AsyncHashSet<T> CreateViaGeneratorAsync<T>(
        int size,
        Func<object> factory,
        IAsyncEqualityComparer<T> comparer,
        int iterationLimit,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(factory);

        async Task<IDictionary<int, IList<T>>> setInitialContentsAsync()
        {
            Dictionary<int, IList<T>> results = [];

            int currentSize = 0;
            for (int i = 0; currentSize < size; i++)
            {
                ArgumentGuard.ThrowUponIterationLimit(i, iterationLimit);

                canceler.ThrowIfCancellationRequested();
                if (
                    await AsyncHashSet<T>
                        .AddToAsync(results, (T)factory.Invoke(), comparer, canceler)
                        .ConfigureAwait(false)
                )
                {
                    currentSize += 1;
                }
            }

            return results;
        }

        return new AsyncHashSet<T>(setInitialContentsAsync(), comparer);
    }
}
