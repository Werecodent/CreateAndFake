using System.Diagnostics;

namespace CreateAndFake.Design.Reiteration;

/// <inheritdoc cref="ILimiter"/>
public sealed partial class Limiter : IAsyncLimiter
{
    /// <inheritdoc/>
    public Task RepeatAsync(string message, Action? behavior, CancellationToken? canceler = null)
    {
        return RepeatAsync(
            message,
            () =>
            {
                behavior?.Invoke();
                return true;
            },
            canceler
        );
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<T>> RepeatAsync<T>(
        string message,
        Func<T> behavior,
        CancellationToken? canceler = null
    )
    {
        ArgumentGuard.ThrowIfNull(behavior, nameof(behavior));

        int i = 0;
        List<T> results = [];
        Stopwatch watch = Stopwatch.StartNew();
        do
        {
            results.Add(behavior.Invoke());
        } while (
            await DelayIfNotDoneAsync(message, watch.Elapsed, ++i, canceler).ConfigureAwait(false)
        );

        return results.AsReadOnly();
    }

    /// <inheritdoc/>
    public Task StallUntilAsync(
        string message,
        Func<bool> behavior,
        CancellationToken? canceler = null
    )
    {
        return StallUntilAsync(message, null, behavior, canceler);
    }

    /// <inheritdoc/>
    public Task StallUntilAsync(
        string message,
        Action? behavior,
        Func<bool> checkState,
        CancellationToken? canceler = null
    )
    {
        return StallUntilAsync(
            message,
            () =>
            {
                behavior?.Invoke();
                return true;
            },
            checkState,
            canceler
        );
    }

    /// <inheritdoc/>
    public Task<IReadOnlyCollection<T>> StallUntilAsync<T>(
        string message,
        Func<T> behavior,
        Func<bool> checkState,
        CancellationToken? canceler = null
    )
    {
        ArgumentGuard.ThrowIfNull(checkState, nameof(checkState));

        return StallUntilAsync(message, behavior, _ => checkState.Invoke(), canceler);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<T>> StallUntilAsync<T>(
        string message,
        Func<T> behavior,
        Func<T, bool> checkState,
        CancellationToken? canceler = null
    )
    {
        ArgumentGuard.ThrowIfNull(behavior, nameof(behavior));
        ArgumentGuard.ThrowIfNull(checkState, nameof(checkState));

        List<T> results = [];
        Stopwatch watch = Stopwatch.StartNew();
        for (int i = 1; true; i++)
        {
            T result = behavior.Invoke();

            results.Add(result);
            if (checkState.Invoke(result))
            {
                break;
            }
            await DelayOrFaultAsync(message, watch.Elapsed, i, canceler).ConfigureAwait(false);
        }

        return results.AsReadOnly();
    }

    /// <inheritdoc/>
    public Task RetryAsync(string message, Action behavior, CancellationToken? canceler = null)
    {
        return RetryAsync<Exception>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public Task RetryAsync(
        string message,
        Action behavior,
        Action resetState,
        CancellationToken? canceler = null
    )
    {
        return RetryAsync<Exception>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public Task RetryAsync<TError>(
        string message,
        Action behavior,
        CancellationToken? canceler = null
    )
        where TError : Exception
    {
        return RetryAsync<TError>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public Task RetryAsync<TError>(
        string message,
        Action behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception
    {
        return RetryAsync<TError, bool>(
            message,
            () =>
            {
                behavior?.Invoke();
                return true;
            },
            resetState,
            canceler
        );
    }

    /// <inheritdoc/>
    public Task<TResult> RetryAsync<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
    {
        return RetryAsync<Exception, TResult>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public Task<TResult> RetryAsync<TResult>(
        string message,
        Func<TResult> behavior,
        Action resetState,
        CancellationToken? canceler = null
    )
    {
        return RetryAsync<Exception, TResult>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public Task<TResult> RetryAsync<TError, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
        where TError : Exception
    {
        return RetryAsync<TError, TResult>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public async Task<TResult> RetryAsync<TError, TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception
    {
        ArgumentGuard.ThrowIfNull(behavior, nameof(behavior));

        Stopwatch watch = Stopwatch.StartNew();
        for (int i = 1; true; i++)
        {
            TError lastError;
            try
            {
                return behavior.Invoke();
            }
            catch (TError error)
            {
                lastError = error;
            }
            resetState?.Invoke();
            await DelayOrFaultAsync(message, watch.Elapsed, i, canceler, lastError)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public Task AttemptAsync(string message, Action behavior, CancellationToken? canceler = null)
    {
        return AttemptAsync<Exception>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public Task AttemptAsync(
        string message,
        Action behavior,
        Action resetState,
        CancellationToken? canceler = null
    )
    {
        return AttemptAsync<Exception>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public Task AttemptAsync<TError>(
        string message,
        Action behavior,
        CancellationToken? canceler = null
    )
        where TError : Exception
    {
        return AttemptAsync<TError>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public Task AttemptAsync<TError>(
        string message,
        Action? behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception
    {
        return AttemptAsync<TError, bool>(
            message,
            () =>
            {
                behavior?.Invoke();
                return true;
            },
            resetState,
            canceler
        );
    }

    /// <inheritdoc/>
    public Task<TResult?> AttemptAsync<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
    {
        return AttemptAsync<Exception, TResult>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public Task<TResult?> AttemptAsync<TResult>(
        string message,
        Func<TResult> behavior,
        Action resetState,
        CancellationToken? canceler = null
    )
    {
        return AttemptAsync<Exception, TResult>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public Task<TResult?> AttemptAsync<TError, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
        where TError : Exception
    {
        return AttemptAsync<TError, TResult>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public async Task<TResult?> AttemptAsync<TError, TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception
    {
        ArgumentGuard.ThrowIfNull(behavior, nameof(behavior));

        int i = 0;
        Stopwatch watch = Stopwatch.StartNew();
        do
        {
            try
            {
                return behavior.Invoke();
            }
            catch (TError) { }

            resetState?.Invoke();
        } while (
            await DelayIfNotDoneAsync(message, watch.Elapsed, ++i, canceler).ConfigureAwait(false)
        );

        return default;
    }

    /// <returns>Awaitable <see cref="Task"/> handling the delay.</returns>
    /// <inheritdoc cref="DelayIfNotDone"/>
    private async Task<bool> DelayIfNotDoneAsync(
        string message,
        TimeSpan elapsed,
        int tries,
        CancellationToken? canceler
    )
    {
        if (tries < _tries && elapsed < _timeout)
        {
            await DelayOrCancelAsync(message, canceler).ConfigureAwait(false);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <returns>Awaitable <see cref="Task"/> handling the delay.</returns>
    /// <inheritdoc cref="DelayOrFault"/>
    private async Task DelayOrFaultAsync(
        string message,
        TimeSpan elapsed,
        int tries,
        CancellationToken? canceler,
        Exception? ex = null
    )
    {
        if (tries >= _tries)
        {
            Fault($"Reached max Attempts of '{_tries}'", message, ex);
        }
        else if (elapsed >= _timeout)
        {
            Fault($"Reached timeout of '{_timeout}'", message, ex);
        }
        else
        {
            await DelayOrCancelAsync(message, canceler).ConfigureAwait(false);
        }
    }

    /// <returns>Awaitable <see cref="Task"/> handling the delay.</returns>
    /// <inheritdoc cref="DelayOrCancel"/>
    private async Task DelayOrCancelAsync(string message, CancellationToken? canceler)
    {
        CancellationToken token = canceler ?? CancellationToken.None;
        try
        {
            if (_delay > TimeSpan.Zero)
            {
                await Task.Delay(_delay, token).ConfigureAwait(false);
            }
            else
            {
                token.ThrowIfCancellationRequested();
            }
        }
        catch (OperationCanceledException e)
        {
            Fault("Operation canceled via token", message, e);
        }
    }
}
