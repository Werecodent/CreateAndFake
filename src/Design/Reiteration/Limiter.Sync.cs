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

        int i = 0;
        List<T> results = [];
        Stopwatch watch = Stopwatch.StartNew();
        do
        {
            results.Add(behavior.Invoke());
        } while (DelayIfNotDone(message, watch.Elapsed, ++i, canceler));

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
        } while (DelayIfNotDone(message, watch.Elapsed, ++i, canceler));

        return default;
    }

    /// <summary>Delays if terminal condition not reached.</summary>
    /// <param name="message">Details to include upon a <see cref="TimeoutException"/>.</param>
    /// <param name="elapsed">Current amount of time that has elapsed.</param>
    /// <param name="tries">Current number of repetitions.</param>
    /// <param name="canceler">Token indicating behavior should be canceled.</param>
    /// <returns>
    ///     <see langword="true"/> if terminal condition not reached,
    ///     <see langword="false"/> otherwise.
    /// </returns>
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
    /// <param name="message">Details to include upon a <see cref="TimeoutException"/>.</param>
    /// <param name="elapsed">Current amount of time that has elapsed.</param>
    /// <param name="tries">Current number of repetitions.</param>
    /// <param name="canceler">Token indicating behavior should be canceled.</param>
    /// <param name="ex">Current <see cref="Exception"/> if present.</param>
    /// <exception cref="TimeoutException">If the limit is reached.</exception>
    /// <exception cref="TimeoutException">If cancelled via <paramref name="canceler"/>.</exception>
    private void DelayOrFault(
        string message,
        TimeSpan elapsed,
        int tries,
        CancellationToken? canceler,
        Exception? ex = null
    )
    {
        if (tries >= _tries)
        {
            Fault($"Reached max tries of '{_tries}'", message, ex);
        }
        else if (elapsed >= _timeout)
        {
            Fault($"Reached timeout of '{_timeout}'", message, ex);
        }
        else
        {
            DelayOrCancel(message, canceler);
        }
    }

    /// <summary>Faults if behavior has been canceled; delays otherwise.</summary>
    /// <param name="message">Details to include upon a <see cref="TimeoutException"/>.</param>
    /// <param name="canceler">Token indicating behavior should be canceled.</param>
    /// <exception cref="TimeoutException">If cancelled via <paramref name="canceler"/>.</exception>
    private void DelayOrCancel(string message, CancellationToken? canceler)
    {
        CancellationToken token = canceler ?? CancellationToken.None;
        if (_delay > TimeSpan.Zero)
        {
            Thread.Sleep(_delay);
            //_ = token.WaitHandle.WaitOne(_delay);
        }
        if (token.IsCancellationRequested)
        {
            Fault("Operation canceled via token", message);
        }
    }
}
