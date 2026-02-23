namespace CreateAndFake.Design.Content;

/// <summary>Provides common <see cref="IAsyncEnumerable{T}"/> patterns.</summary>
/// <typeparam name="T">
///     <inheritdoc cref="IAsyncEnumerable{T}" path="/typeparam[@name='T']"/>
/// </typeparam>
public static class AsyncSeriesHelper<T>
{
    /// <summary>Cached series with no elements.</summary>
    public static IAsyncEnumerable<T> Empty { get; } = new EmptyAsyncEnumerator();

    /// <summary>Represents a series with no elements.</summary>
    private sealed class EmptyAsyncEnumerator : IAsyncEnumerable<T>, IAsyncEnumerator<T>
    {
        /// <inheritdoc/>
        public T Current => default!;

        /// <inheritdoc/>
        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(false);
        }

        /// <inheritdoc/>
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return this;
        }

        /// <inheritdoc/>
        public ValueTask DisposeAsync()
        {
            return default;
        }
    }
}
