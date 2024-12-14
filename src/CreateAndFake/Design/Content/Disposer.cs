namespace CreateAndFake.Design.Content;

/// <summary>Disposes generated objects.</summary>
internal static class Disposer
{
    /// <summary>Tries to dispose <paramref name="items"/>.</summary>
    /// <param name="items">Potential disposables to clean up.</param>
    internal static void Cleanup(params IEnumerable<object?>? items)
    {
        foreach (object? item in items ?? [])
        {
            (item as IDisposable)?.Dispose();
            _ = (item as IAsyncDisposable)?.DisposeAsync().Preserve();
        }
    }
}