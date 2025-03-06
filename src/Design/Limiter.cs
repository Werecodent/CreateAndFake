using System.Diagnostics;
using CreateAndFake.Design.Content;

namespace CreateAndFake.Design;

/// <summary>Provides the core functionality for repetition.</summary>
/// <param name="timeout"><inheritdoc cref="_timeout" path="/summary"/></param>
/// <param name="tries"><inheritdoc cref="_tries" path="/summary"/></param>
/// <param name="delay"><inheritdoc cref="_delay" path="/summary"/></param>
public sealed class Limiter(TimeSpan timeout, int tries, TimeSpan? delay = null)
    : IEquatable<Limiter>
{
    /// <summary>Instance that defaults to a single attempt.</summary>
    public static Limiter Once { get; } = new Limiter(1);

    /// <summary>Instance that defaults to five attempts.</summary>
    public static Limiter Few { get; } = new Limiter(5);

    /// <summary>Instance that defaults to a dozen attempts.</summary>
    public static Limiter Dozen { get; } = new Limiter(12);

    /// <summary>Instance that defaults to a twenty attempts.</summary>
    public static Limiter Score { get; } = new Limiter(20);

    /// <summary>Instance that defaults to a hundred attempts.</summary>
    public static Limiter Hundred { get; } = new Limiter(100);

    /// <summary>Instance that defaults to a thousand attempts.</summary>
    public static Limiter Myriad { get; } = new Limiter(1000);

    /// <summary>Instance that defaults to 25 ms with a minimal delay.</summary>
    public static Limiter Quick { get; } =
        new Limiter(new TimeSpan(0, 0, 0, 0, 25), new TimeSpan(0, 0, 0, 0, 1));

    /// <summary>Instance that defaults to half a second with a small delay.</summary>
    public static Limiter Fast { get; } =
        new Limiter(new TimeSpan(0, 0, 0, 0, 500), new TimeSpan(0, 0, 0, 0, 20));

    /// <summary>Instance that defaults to five seconds with a large delay.</summary>
    public static Limiter Slow { get; } =
        new Limiter(new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 0, 0, 200));

    /// <summary>Maximum attempts to try.</summary>
    private readonly int _tries = tries;

    /// <summary>Maximum duration to attempt for.</summary>
    private readonly TimeSpan _timeout = timeout;

    /// <summary>Delay between attempts.</summary>
    private readonly TimeSpan _delay = delay ?? TimeSpan.Zero;

    /// <inheritdoc cref="Limiter(TimeSpan,int,TimeSpan?)"/>
    public Limiter(int tries, TimeSpan? delay = null)
        : this(TimeSpan.MaxValue, tries, delay) { }

    /// <inheritdoc cref="Limiter(TimeSpan,int,TimeSpan?)"/>
    public Limiter(TimeSpan timeout, TimeSpan? delay = null)
        : this(timeout, int.MaxValue, delay) { }

    /// <returns>Awaitable <c>Task</c> handling the repetitions.</returns>
    /// <inheritdoc cref="Repeat{T}(string,Func{T},CancellationToken?)"/>
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

    /// <summary>Repeats <paramref name="behavior"/>.</summary>
    /// <typeparam name="T"><c>Type</c> returned from <paramref name="behavior"/>.</typeparam>
    /// <param name="message">Details to include upon a <see cref="TimeoutException"/>.</param>
    /// <param name="behavior">Behavior to repeatably do.</param>
    /// <param name="canceler">Token to potentially cancel the attempts.</param>
    /// <returns>Results from all calls.</returns>
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

    /// <summary>Retries <paramref name="behavior"/> until it returns <c>true</c>.</summary>
    /// <inheritdoc cref="StallUntil(string,Action,Func{bool},CancellationToken?)"/>
    public void StallUntil(string message, Func<bool> behavior, CancellationToken? canceler = null)
    {
        StallUntil(message, null, behavior, canceler);
    }

    /// <returns>Awaitable <c>Task</c> handling the repetitions.</returns>
    /// <inheritdoc cref="StallUntil{T}(string,Func{T},Func{bool},CancellationToken?)"/>
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

    /// <inheritdoc cref="StallUntil{T}(string,Func{T},Func{T,bool},CancellationToken?)"/>
    public IReadOnlyCollection<T> StallUntil<T>(
        string message,
        Func<T> behavior,
        Func<bool> checkState,
        CancellationToken? canceler = null
    )
    {
        ArgumentGuard.ThrowIfNull(checkState, nameof(checkState));

        return StallUntil(message, behavior, (T _) => checkState.Invoke(), canceler);
    }

    /// <summary>Retries <paramref name="behavior"/> until <paramref name="checkState"/>.</summary>
    /// <typeparam name="T">Result <c>Type</c> returned from <paramref name="behavior"/>.</typeparam>
    /// <param name="message">Details to include upon a <see cref="TimeoutException"/>.</param>
    /// <param name="behavior">Behavior to repeatably attempt.</param>
    /// <param name="checkState">Polls if the behavior was successful.</param>
    /// <param name="canceler">Token to potentially cancel the attempts.</param>
    /// <returns>Results from all calls.</returns>
    /// <exception cref="TimeoutException">If an attempt limit is reached.</exception>
    /// <remarks>Beware infinite loops in <paramref name="behavior"/>.</remarks>
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

    /// <inheritdoc cref="Retry{T}(string,Action,Action,CancellationToken?)"/>
    public void Retry(string message, Action behavior, CancellationToken? canceler = null)
    {
        Retry<Exception>(message, behavior, null, canceler);
    }

    /// <inheritdoc cref="Retry{T}(string,Action,Action,CancellationToken?)"/>
    public void Retry(
        string message,
        Action behavior,
        Action resetState,
        CancellationToken? canceler = null
    )
    {
        Retry<Exception>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc cref="Retry{T}(string,Action,Action,CancellationToken?)"/>
    public void Retry<TException>(
        string message,
        Action behavior,
        CancellationToken? canceler = null
    )
        where TException : Exception
    {
        Retry<TException>(message, behavior, null, canceler);
    }

    /// <returns>Awaitable <c>Task</c> handling the repetitions.</returns>
    /// <inheritdoc cref="Retry{TException,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    public void Retry<TException>(
        string message,
        Action behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TException : Exception
    {
        _ = Retry<TException, bool>(
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

    /// <inheritdoc cref="Retry{TException,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    public TResult Retry<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
    {
        return Retry<Exception, TResult>(message, behavior, null, canceler);
    }

    /// <inheritdoc cref="Retry{TException,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    public TResult Retry<TResult>(
        string message,
        Func<TResult> behavior,
        Action resetState,
        CancellationToken? canceler = null
    )
    {
        return Retry<Exception, TResult>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc cref="Retry{TException,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    public TResult Retry<TException, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
        where TException : Exception
    {
        return Retry<TException, TResult>(message, behavior, null, canceler);
    }

    /// <returns>Result of the successful <paramref name="behavior"/> attempt.</returns>
    /// <exception cref="TimeoutException">If an attempt limit is reached.</exception>
    /// <inheritdoc cref="Attempt{TException,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    public TResult Retry<TException, TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(behavior, nameof(behavior));

        Stopwatch watch = Stopwatch.StartNew();
        for (int i = 1; true; i++)
        {
            TException lastError;
            try
            {
                return behavior.Invoke();
            }
            catch (TException error)
            {
                lastError = error;
            }
            resetState?.Invoke();
            DelayOrFault(message, watch.Elapsed, i, canceler, lastError);
        }
    }

    /// <inheritdoc cref="Attempt{T}(string,Action,Action,CancellationToken?)"/>
    public void Attempt(string message, Action behavior, CancellationToken? canceler = null)
    {
        Attempt<Exception>(message, behavior, null, canceler);
    }

    /// <inheritdoc cref="Attempt{T}(string,Action,Action,CancellationToken?)"/>
    public void Attempt(
        string message,
        Action behavior,
        Action resetState,
        CancellationToken? canceler = null
    )
    {
        Attempt<Exception>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc cref="Attempt{T}(string,Action,Action,CancellationToken?)"/>
    public void Attempt<TException>(
        string message,
        Action behavior,
        CancellationToken? canceler = null
    )
        where TException : Exception
    {
        Attempt<TException>(message, behavior, null, canceler);
    }

    /// <returns>Awaitable task handling the repetitions.</returns>
    /// <inheritdoc cref="Attempt{TException,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    public void Attempt<TException>(
        string message,
        Action? behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TException : Exception
    {
        _ = Attempt<TException, bool>(
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

    /// <inheritdoc cref="Attempt{TException,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    public TResult? Attempt<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
    {
        return Attempt<Exception, TResult>(message, behavior, null, canceler);
    }

    /// <inheritdoc cref="Attempt{TException,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    public TResult? Attempt<TResult>(
        string message,
        Func<TResult> behavior,
        Action resetState,
        CancellationToken? canceler = null
    )
    {
        return Attempt<Exception, TResult>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc cref="Attempt{TException,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    public TResult? Attempt<TException, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
        where TException : Exception
    {
        return Attempt<TException, TResult>(message, behavior, null, canceler);
    }

    /// <summary>
    ///     Retries <paramref name="behavior"/> when encountering
    ///     exceptions of <c>Type</c> <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="TException">
    ///     Exception <c>Type</c> to ignore and retry <paramref name="behavior"/> if encountered.
    /// </typeparam>
    /// <typeparam name="TResult">Result <c>Type</c> returned from <paramref name="behavior"/>.</typeparam>
    /// <param name="message">Details to include upon a <see cref="TimeoutException"/>.</param>
    /// <param name="behavior">Behavior to repeatably attempt.</param>
    /// <param name="resetState">Behavior to reset state before next attempt.</param>
    /// <param name="canceler">Token to potentially cancel the attempts.</param>
    /// <returns>
    ///     Result of the successful <paramref name="behavior"/> attempt or <c>default</c> if limit reached.
    /// </returns>
    /// <exception cref="TimeoutException">If an attempt limit is reached.</exception>
    /// <remarks>Beware infinite loops in <paramref name="behavior"/>.</remarks>
    public TResult? Attempt<TException, TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TException : Exception
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
            catch (TException) { }

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
    /// <returns>Awaitable <c>Task</c> handling the delay.</returns>
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
    /// <returns>Awaitable <c>Task</c> handling the delay.</returns>
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

    /// <summary>Compares <c>this</c> to <paramref name="obj"/> by value.</summary>
    /// <param name="obj">Instance to compare <c>this</c> with.</param>
    /// <returns>
    ///     <c>true</c> if <c>this</c> is equal to <paramref name="obj"/> by value; <c>false</c> otherwise.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as Limiter);
    }

    /// <inheritdoc cref="IValueEquatable.ValuesEqual"/>
    public bool Equals(Limiter? other)
    {
        return other != null
            && other._delay == _delay
            && other._tries == _tries
            && other._timeout == _timeout;
    }

    /// <inheritdoc cref="IValueEquatable.GetValueHash"/>
    public override int GetHashCode()
    {
        return ValueComparer.Use.GetHashCode(_tries, _timeout, _delay);
    }

    /// <summary>Converts <c>this</c> to a <c>string</c>.</summary>
    /// <returns><c>string</c> representation of <c>this</c>.</returns>
    public override string ToString()
    {
        return $"{_tries}-{_timeout}-{_delay}";
    }
}
