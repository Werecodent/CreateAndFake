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
            IDictionary<int, IList<T>> results = new Dictionary<int, IList<T>>();

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
            AsyncSeriesHelper.CreateFromAsync(contents, iterationLimit, canceler),
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
}
