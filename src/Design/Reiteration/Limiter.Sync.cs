using System.Diagnostics;

namespace CreateAndFake.Design.Reiteration;

/// <inheritdoc cref="ILimiter"/>
public sealed partial class Limiter : ISyncLimiter
{
    /// <inheritdoc/>
    public void Repeat(string message, Action? behavior, CancellationToken? canceler = null)
    {
        _ = Repeat(
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
    public IReadOnlyCollection<T> Repeat<T>(
        string message,
        Func<T> behavior,
        CancellationToken? canceler = null
    )
    {
        ArgumentGuard.ThrowIfNull(behavior, nameof(behavior));

        List<T> results = [];

        Stopwatch watch = Stopwatch.StartNew();
        int attempt = 0;
        do
        {
            results.Add(behavior.Invoke());
        } while (DelayIfNotDone(message, watch.Elapsed, ++attempt, canceler));

        return results.AsReadOnly();
    }

    /// <inheritdoc/>
    public void StallUntil(string message, Func<bool> behavior, CancellationToken? canceler = null)
    {
        StallUntil(message, null, behavior, canceler);
    }

    /// <inheritdoc/>
    public void StallUntil(
        string message,
        Action? behavior,
        Func<bool> checkState,
        CancellationToken? canceler = null
    )
    {
        _ = StallUntil(
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
    public IReadOnlyCollection<T> StallUntil<T>(
        string message,
        Func<T> behavior,
        Func<bool> checkState,
        CancellationToken? canceler = null
    )
    {
        ArgumentGuard.ThrowIfNull(checkState, nameof(checkState));

        return StallUntil(message, behavior, _ => checkState.Invoke(), canceler);
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<T> StallUntil<T>(
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
            else
            {
                DelayOrFault(message, watch.Elapsed, i, canceler);
            }
        }

        return results.AsReadOnly();
    }

    /// <inheritdoc/>
    public void Retry(string message, Action behavior, CancellationToken? canceler = null)
    {
        Retry<Exception>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public void Retry(
        string message,
        Action behavior,
        Action resetState,
        CancellationToken? canceler = null
    )
    {
        Retry<Exception>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public void Retry<TError>(string message, Action behavior, CancellationToken? canceler = null)
        where TError : Exception
    {
        Retry<TError>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public void Retry<TError>(
        string message,
        Action behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception
    {
        _ = Retry<TError, bool>(
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
    public TResult Retry<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
    {
        return Retry<Exception, TResult>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public TResult Retry<TResult>(
        string message,
        Func<TResult> behavior,
        Action resetState,
        CancellationToken? canceler = null
    )
    {
        return Retry<Exception, TResult>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public TResult Retry<TError, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
        where TError : Exception
    {
        return Retry<TError, TResult>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public TResult Retry<TError, TResult>(
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
            DelayOrFault(message, watch.Elapsed, i, canceler, lastError);
        }
    }

    /// <inheritdoc/>
    public void Attempt(string message, Action behavior, CancellationToken? canceler = null)
    {
        Attempt<Exception>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public void Attempt(
        string message,
        Action behavior,
        Action resetState,
        CancellationToken? canceler = null
    )
    {
        Attempt<Exception>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public void Attempt<TError>(string message, Action behavior, CancellationToken? canceler = null)
        where TError : Exception
    {
        Attempt<TError>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public void Attempt<TError>(
        string message,
        Action? behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception
    {
        _ = Attempt<TError, bool>(
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
    public TResult? Attempt<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
    {
        return Attempt<Exception, TResult>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public TResult? Attempt<TResult>(
        string message,
        Func<TResult> behavior,
        Action resetState,
        CancellationToken? canceler = null
    )
    {
        return Attempt<Exception, TResult>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public TResult? Attempt<TError, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
        where TError : Exception
    {
        return Attempt<TError, TResult>(message, behavior, null, canceler);
    }

    /// <inheritdoc/>
    public TResult? Attempt<TError, TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception
    {
        ArgumentGuard.ThrowIfNull(behavior, nameof(behavior));

        Stopwatch watch = Stopwatch.StartNew();
        int i = 0;
        do
        {
            try
            {
                return behavior.Invoke();
            }
            catch (TError) { }

            resetState?.Invoke();
        } while (DelayIfNotDone(message, watch.Elapsed, ++i, canceler));

        return default;
    }

    /// <summary>Delays if terminal condition not reached.</summary>
    /// <param name="message">Details to include upon a <see cref="TimeoutException"/>.</param>
    /// <param name="elapsed">Current amount of time that has elapsed.</param>
    /// <param name="tries">Current number of attempts.</param>
    /// <param name="canceler">Token to potentially cancel the attempts.</param>
    /// <returns><c>true</c> if terminal condition not reached; <c>false</c> otherwise.</returns>
    private bool DelayIfNotDone(
        string message,
        TimeSpan elapsed,
        int tries,
        CancellationToken? canceler
    )
    {
        if (tries < _tries && elapsed < _timeout)
        {
            DelayOrCancel(message, canceler);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>Faults if terminal condition reached; delays otherwise.</summary>
    /// <param name="message">Details to include upon a timeout <c>exception</c>.</param>
    /// <param name="elapsed">Current amount of time that has elapsed.</param>
    /// <param name="tries">Current number of attempts.</param>
    /// <param name="canceler">Token to potentially cancel the attempts.</param>
    /// <param name="ex">Current <c>exception</c> if present.</param>
    /// <exception cref="TimeoutException">If an attempt limit is reached.</exception>
    private void DelayOrFault(
        string message,
        TimeSpan elapsed,
        int tries,
        CancellationToken? canceler,
        Exception? ex = null
    )
    {
        string details = string.IsNullOrWhiteSpace(message) ? "." : $": {message}";
        if (tries >= _tries)
        {
            throw new TimeoutException($"Reached max attempts of '{_tries}'{details}", ex);
        }
        else if (elapsed >= _timeout)
        {
            throw new TimeoutException($"Reached timeout of '{_timeout}'{details}", ex);
        }
        else
        {
            DelayOrCancel(message, canceler);
        }
    }

    /// <summary>Faults if behavior has been canceled; delays otherwise.</summary>
    /// <param name="message">Details to include upon a timeout <c>exception</c>.</param>
    /// <param name="canceler">Token to potentially cancel the attempts.</param>
    private void DelayOrCancel(string message, CancellationToken? canceler)
    {
        CancellationToken token = canceler ?? CancellationToken.None;
        if (_delay > TimeSpan.Zero)
        {
            _ = token.WaitHandle.WaitOne(_delay);
        }
        if (token.IsCancellationRequested)
        {
            string details = string.IsNullOrWhiteSpace(message) ? "." : $": {message}";
            throw new TimeoutException($"Operation canceled via token{details}");
        }
    }
}
