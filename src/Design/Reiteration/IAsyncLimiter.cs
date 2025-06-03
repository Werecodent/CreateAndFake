namespace CreateAndFake.Design.Reiteration;

/// <summary>Provides the core functionality for asynchronous repetition.</summary>
public interface IAsyncLimiter
{
    /// <returns>Awaitable <see cref="Task"/> handling the repetitions.</returns>
    /// <inheritdoc cref="RepeatAsync{T}(string,Func{T},CancellationToken?)"/>
    Task RepeatAsync(string message, Action? behavior, CancellationToken? canceler = null);

    /// <inheritdoc cref="ISyncLimiter.Repeat{T}(string,Func{T},CancellationToken?)"/>
    Task<IReadOnlyCollection<T>> RepeatAsync<T>(
        string message,
        Func<T> behavior,
        CancellationToken? canceler = null
    );

    /// <returns>Awaitable <see cref="Task"/> handling the repetitions.</returns>
    /// <inheritdoc cref="ISyncLimiter.StallUntil(string,Func{bool},CancellationToken?)"/>
    Task StallUntilAsync(string message, Func<bool> behavior, CancellationToken? canceler = null);

    /// <returns>Awaitable <see cref="Task"/> handling the repetitions.</returns>
    /// <inheritdoc cref="StallUntilAsync{T}(string,Func{T},Func{bool},CancellationToken?)"/>
    Task StallUntilAsync(
        string message,
        Action? behavior,
        Func<bool> checkState,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="StallUntilAsync{T}(string,Func{T},Func{T,bool},CancellationToken?)"/>
    Task<IReadOnlyCollection<T>> StallUntilAsync<T>(
        string message,
        Func<T> behavior,
        Func<bool> checkState,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="ISyncLimiter.StallUntil{T}(string,Func{T},Func{bool},CancellationToken?)"/>
    Task<IReadOnlyCollection<T>> StallUntilAsync<T>(
        string message,
        Func<T> behavior,
        Func<T, bool> checkState,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="RetryAsync{T}(string,Action,Action,CancellationToken?)"/>
    Task RetryAsync(string message, Action behavior, CancellationToken? canceler = null);

    /// <inheritdoc cref="RetryAsync{T}(string,Action,Action,CancellationToken?)"/>
    Task RetryAsync(
        string message,
        Action behavior,
        Action resetState,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="RetryAsync{T}(string,Action,Action,CancellationToken?)"/>
    Task RetryAsync<TError>(string message, Action behavior, CancellationToken? canceler = null)
        where TError : Exception;

    /// <returns>Awaitable <see cref="Task"/> handling the repetitions.</returns>
    /// <inheritdoc cref="RetryAsync{T,T}(string,Func{T},Action,CancellationToken?)"/>
    Task RetryAsync<TError>(
        string message,
        Action behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception;

    /// <inheritdoc cref="RetryAsync{T,T}(string,Func{T},Action,CancellationToken?)"/>
    Task<TResult> RetryAsync<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="RetryAsync{T,T}(string,Func{T},Action,CancellationToken?)"/>
    Task<TResult> RetryAsync<TResult>(
        string message,
        Func<TResult> behavior,
        Action resetState,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="RetryAsync{T,T}(string,Func{T},Action,CancellationToken?)"/>
    Task<TResult> RetryAsync<TError, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
        where TError : Exception;

    /// <inheritdoc cref="ISyncLimiter.Retry{T,T}(string,Func{T},Action,CancellationToken?)"/>
    Task<TResult> RetryAsync<TError, TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception;

    /// <inheritdoc cref="AttemptAsync{T}(string,Action,Action,CancellationToken?)"/>
    Task AttemptAsync(string message, Action behavior, CancellationToken? canceler = null);

    /// <inheritdoc cref="AttemptAsync{T}(string,Action,Action,CancellationToken?)"/>
    Task AttemptAsync(
        string message,
        Action behavior,
        Action resetState,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="AttemptAsync{T}(string,Action,Action,CancellationToken?)"/>
    Task AttemptAsync<TError>(string message, Action behavior, CancellationToken? canceler = null)
        where TError : Exception;

    /// <returns>Awaitable <see cref="Task"/> handling the repetitions.</returns>
    /// <inheritdoc cref="AttemptAsync{T,T}(string,Func{T},Action,CancellationToken?)"/>
    Task AttemptAsync<TError>(
        string message,
        Action? behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception;

    /// <inheritdoc cref="AttemptAsync{T,T}(string,Func{T},Action,CancellationToken?)"/>
    Task<TResult?> AttemptAsync<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="AttemptAsync{T,T}(string,Func{T},Action,CancellationToken?)"/>
    Task<TResult?> AttemptAsync<TResult>(
        string message,
        Func<TResult> behavior,
        Action resetState,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="AttemptAsync{T,T}(string,Func{T},Action,CancellationToken?)"/>
    Task<TResult?> AttemptAsync<TError, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
        where TError : Exception;

    /// <inheritdoc cref="ISyncLimiter.Attempt{T,T}(string,Func{T},Action,CancellationToken?)"/>
    Task<TResult?> AttemptAsync<TError, TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception;
}
