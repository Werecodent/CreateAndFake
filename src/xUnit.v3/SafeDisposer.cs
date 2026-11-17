using Werecodent.CreateAndFake.Design;

namespace Werecodent.CreateAndFake.xUnit.v3;

#pragma warning disable CA1816 // To prevent crashing the entire program.

/// <summary>Handles disposing randomly generated data with logging for failures.</summary>
internal sealed class SafeDisposer : IAsyncDisposable
{
    /// <summary>Name of the method the data was generated for.</summary>
    private readonly string _methodName;

    /// <summary>Generated synchronous disposables to handle.</summary>
    private readonly IEnumerable<IDisposable> _disposables;

    /// <summary>Generated asynchronous disposables to handle.</summary>
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
            catch (Exception e)
            {
                // Prevent test harness crashes.
                GC.SuppressFinalize(current);

                await Console
                    .Error.WriteLineAsync($"Disposal failure from {_methodName}= {e.Message}")
                    .ConfigureAwait(false);
            }
        }
        foreach (IAsyncDisposable current in _asyncDisposables)
        {
            try
            {
                await current.DisposeAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                // Prevent test harness crashes.
                GC.SuppressFinalize(current);

                await Console
                    .Error.WriteLineAsync($"Disposal failure from {_methodName}= {e.Message}")
                    .ConfigureAwait(false);
            }
        }
    }

    /// <summary><inheritdoc cref="SafeDisposer" path="/summary"/></summary>
    /// <param name="methodName"><inheritdoc cref="_methodName" path="/summary"/></param>
    /// <param name="disposables"><inheritdoc cref="_disposables" path="/summary"/></param>
    /// <param name="asyncDisposables"><inheritdoc cref="_asyncDisposables" path="/summary"/></param>
    private SafeDisposer(
        string methodName,
        IEnumerable<IDisposable> disposables,
        IEnumerable<IAsyncDisposable> asyncDisposables
    )
    {
        _methodName = methodName;
        _disposables = disposables;
        _asyncDisposables = asyncDisposables;
    }

    /// <summary>Wraps all disposables from the <paramref name="items"/> if any.</summary>
    /// <param name="methodName"><inheritdoc cref="_methodName" path="/summary"/></param>
    /// <param name="items">Generated data for the method with potential disposables.</param>
    /// <returns>Instance if there are any disposables, <see langword="null"/> if none.</returns>
    public static SafeDisposer? TryTracking(string methodName, IEnumerable<object?> items)
    {
        ArgumentGuard.ThrowIfNull(items);

        object?[] disposables = [.. items.Where(item => item is IDisposable or IAsyncDisposable)];

        if (disposables.Length != 0)
        {
            return new(
                methodName,
                disposables.OfType<IDisposable>(),
                disposables.OfType<IAsyncDisposable>()
            );
        }
        else
        {
            return null;
        }
    }
}

#pragma warning restore CA1816 // To prevent crashing the entire program.
