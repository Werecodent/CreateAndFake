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
            (item as IDisposable)?.Dispose();
            _ = (item as IAsyncDisposable)?.DisposeAsync().AsTask();
        }
    }

    /// <returns>Awaitable <see cref="Task"/> handling the disposal.</returns>
    /// <inheritdoc cref="Cleanup"/>
    public static async Task CleanupAsync(params IEnumerable<object?>? items)
    {
        foreach (object? item in items ?? [])
        {
            (item as IDisposable)?.Dispose();
            if (item is IAsyncDisposable disposable)
            {
                await disposable.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}
