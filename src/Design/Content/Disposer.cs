namespace Werecodent.CreateAndFake.Design.Content;

/// <summary>Cleans <see cref="IDisposable"/> and <see cref="IAsyncDisposable"/> objects.</summary>
public static class Disposer
{
    /// <summary>Disposes all <see cref="IDisposable"/>s in the <paramref name="series"/>.</summary>
    /// <param name="series">Collection with potential <see cref="IDisposable"/>s to clean up.</param>
    public static void Cleanup(params ICollection<object?>? series)
    {
        foreach (object? item in series ?? [])
        {
            if (item is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

    /// <summary>
    ///     Disposes all <see cref="IAsyncDisposable"/>s and
    ///     <see cref="IDisposable"/>s in the <paramref name="series"/>.
    /// </summary>
    /// <param name="series">
    ///     Collection with potential <see cref="IAsyncDisposable"/>s and/or <see cref="IDisposable"/>s to clean up.
    /// </param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <remarks>Will dispose an instance via <see cref="IAsyncDisposable"/> if both are inherited.</remarks>
    public static async Task CleanupAsync(params ICollection<object?>? series)
    {
        foreach (object? item in series ?? [])
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
