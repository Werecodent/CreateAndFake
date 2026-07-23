using System.Reflection;

namespace CreateAndFake.Design.Content;

/// <summary>Attempts to cancel <see cref="CancellationTokenSource"/>s via <see langword="async"/>.</summary>
internal sealed class MaybeAsyncCanceler
{
    /// <summary>Instance to use for canceling <see cref="CancellationTokenSource"/>s.</summary>
    public static MaybeAsyncCanceler Use { get; } =
        new(typeof(CancellationTokenSource).GetMethod("CancelAsync"));

    /// <summary>Delegate for triggering asynchronous cancellation.</summary>
    /// <remarks><see langword="null"/> when unavailable for the executing .NET version.</remarks>
    private readonly Func<CancellationTokenSource, Task>? _canceler;

    /// <inheritdoc cref="MaybeAsyncCanceler"/>
    /// <param name="tokenAsyncMethod">
    ///     Method for triggering asynchronous cancellation on <see cref="CancellationTokenSource"/>s.
    /// </param>
    /// <remarks>Do not call; exposed to enable testing.</remarks>
    internal MaybeAsyncCanceler(MethodInfo? tokenAsyncMethod)
    {
        _canceler = (Func<CancellationTokenSource, Task>?)
            tokenAsyncMethod?.CreateDelegate(typeof(Func<CancellationTokenSource, Task>));
    }

    /// <summary>
    ///     Handles canceling a token via its <paramref name="source"/> using <see langword="async"/> if possible.
    /// </summary>
    /// <param name="source">Owner of the <see cref="CancellationToken"/> to cancel.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    internal async Task TriggerCancellationAsync(CancellationTokenSource source)
    {
        ArgumentGuard.ThrowIfNull(source);

        if (!source.IsCancellationRequested)
        {
            if (_canceler != null)
            {
                await _canceler(source).ConfigureAwait(false);
            }
            else
            {
#pragma warning disable AsyncFixer02, S6966, CA1849, MA0042, VSTHRD103 // CancelAsync not available.
                source.Cancel();
#pragma warning restore AsyncFixer02, S6966, CA1849, MA0042, VSTHRD103
            }
        }
    }
}
