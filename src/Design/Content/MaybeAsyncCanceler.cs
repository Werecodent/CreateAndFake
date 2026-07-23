using System.Reflection;

namespace CreateAndFake.Design.Content;

/// <summary>Attempts to cancel <see cref="CancellationTokenSource"/>s via <see langword="async"/>.</summary>
/// <param name="tokenAsyncMethod">
///     Method for triggering asynchronous cancellation on <see cref="CancellationTokenSource"/>s.
///     Do not set; exposed to enable testing.
/// </param>
internal sealed class MaybeAsyncCanceler(MethodInfo? tokenAsyncMethod = null)
{
    /// <summary>Delegate for triggering asynchronous cancellation.</summary>
    /// <remarks><see langword="null"/> when unavailable for the executing .NET version.</remarks>
    private readonly Func<CancellationTokenSource, Task>? _canceler = (Func<
        CancellationTokenSource,
        Task
    >?)
        (
            tokenAsyncMethod ?? typeof(CancellationTokenSource).GetMethod("CancelAsync")
        )?.CreateDelegate(typeof(Func<CancellationTokenSource, Task>));

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
