namespace CreateAndFake.Design.Content;

/// <summary>Disposes generated objects.</summary>
public static class Disposer
{
    /// <summary>Tries to dispose <paramref name="items"/>.</summary>
    /// <param name="items">Potential disposables to clean up.</param>
    public static void Cleanup(params IEnumerable<object?>? items)
    {
        foreach (object? item in items ?? [])
        {
            if (item is IDisposable disposable)
            {
                disposable.Dispose();
            }
            else if (item is IAsyncDisposable asyncDisposable)
            {
                _ = asyncDisposable.DisposeAsync().AsTask();
            }
        }
    }

    /// <returns>Awaitable <see cref="Task"/> handling the disposal.</returns>
    /// <inheritdoc cref="Cleanup"/>
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
