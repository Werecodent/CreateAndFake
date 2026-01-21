namespace CreateAndFake.Design.Content;

/// <summary>Cleans <see cref="IDisposable"/> and <see cref="IAsyncDisposable"/> objects.</summary>
public static class Disposer
{
    /// <summary>Disposes all <see cref="IDisposable"/>s in <paramref name="items"/>.</summary>
    /// <param name="items">Series with potential <see cref="IDisposable"/>s to clean up.</param>
    public static void Cleanup(params IEnumerable<object?>? items)
    {
        foreach (object? item in items ?? [])
        {
            if (item is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

    /// <summary>
    ///     Disposes all <see cref="IAsyncDisposable"/>s and
    ///     <see cref="IDisposable"/>s in <paramref name="items"/>.
    /// </summary>
    /// <param name="items">
    ///     Series with potential <see cref="IAsyncDisposable"/>s
    ///     and/or <see cref="IDisposable"/>s to clean up.
    /// </param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     <see cref="IAsyncDisposable"/>s are prioritized over <see cref="IDisposable"/>s.
    /// </remarks>
    public static async Task CleanupAsync(params IEnumerable<object?>? items)
    {
        foreach (object? item in items ?? [])
        {
            if (item is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
            }
            else if (item is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
