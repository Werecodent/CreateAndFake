namespace CreateAndFake.Design.Content;

/// <summary>Provides common <see cref="IAsyncEnumerable{T}"/> patterns.</summary>
/// <typeparam name="T"><see cref="IAsyncEnumerable{T}"/> content type.</typeparam>
public static class AsyncEnumHelper<T>
{
    /// <summary>Cached enumerator with no elements.</summary>
    public static IAsyncEnumerable<T> Empty { get; } = CreateEmpty();

    /// <summary>Creates an enumerator with no elements.</summary>
    /// <returns>The created enumerator.</returns>
    private static async IAsyncEnumerable<T> CreateEmpty()
    {
        yield break;
    }
}
