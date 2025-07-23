namespace CreateAndFake.xUnit.v3;

internal sealed class SafeDisposer : IAsyncDisposable
{
    private readonly IEnumerable<IDisposable> _disposables;

    private readonly IEnumerable<IAsyncDisposable> _asyncDisposables;

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        foreach (IDisposable current in _disposables)
        {
            try
            {
                current.Dispose();
            }
            catch (Exception)
            {
                // Prevent test harness crashes.
            }
        }
        foreach (IAsyncDisposable current in _asyncDisposables)
        {
            try
            {
                await current.DisposeAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                // Prevent test harness crashes.
            }
        }
    }

    private SafeDisposer(
        IEnumerable<IDisposable> disposables,
        IEnumerable<IAsyncDisposable> asyncDisposables
    )
    {
        _disposables = disposables;
        _asyncDisposables = asyncDisposables;
    }

    /// <summary>Test</summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static SafeDisposer? TryTracking(IEnumerable<object?> items)
    {
        object?[] disposables = [.. items.Where(item => item is IDisposable or IAsyncDisposable)];

        if (disposables.Length != 0)
        {
            return new(disposables.OfType<IDisposable>(), disposables.OfType<IAsyncDisposable>());
        }
        else
        {
            return null;
        }
    }
}
