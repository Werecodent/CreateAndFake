namespace CreateAndFake.Design.Content;

/// <summary>Provides common <see cref="IAsyncEnumerable{T}"/> patterns.</summary>
public static class AsyncEnumHelper
{
    /// <summary>Converts <paramref name="values"/> to an async collection.</summary>
    /// <typeparam name="T">Content type.</typeparam>
    /// <param name="values">Collection to convert.</param>
    /// <returns>The created collection.</returns>
    public static async IAsyncEnumerable<T> CreateFrom<T>(IEnumerable<T>? values)
    {
        foreach (T value in values ?? [])
        {
            yield return value;
        }
    }

    /// <summary>Determines if <paramref name="values"/> has any elements.</summary>
    /// <typeparam name="T">Content type.</typeparam>
    /// <param name="values">Collection to check.</param>
    /// <returns>
    ///     <see langword="true"/> if <paramref name="values"/> has
    ///     at least one element, <see langword="false"/> otherwise.
    /// </returns>
    public static async Task<bool> HasAnyAsync<T>(IAsyncEnumerable<T>? values)
    {
        if (values == null)
        {
            return false;
        }
        IAsyncEnumerator<T> enumerator = values.GetAsyncEnumerator();
        return await enumerator.MoveNextAsync().ConfigureAwait(false);
    }

    /// <summary>Converts <paramref name="values"/> to an array.</summary>
    /// <typeparam name="T">Content type.</typeparam>
    /// <param name="values">Collection to convert.</param>
    /// <returns>The created array.</returns>
    public static async Task<T[]> ToArrayAsync<T>(IAsyncEnumerable<T> values)
    {
        List<T> results = [];
        await foreach (T value in values.ConfigureAwait(false))
        {
            results.Add(value);
        }
        return [.. results];
    }
}
