namespace Werecodent.CreateAndFake.Design.Reiteration;

/// <summary>Provides the core functionality for asynchronous repetition.</summary>
public interface ILimiterAsync
{
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <inheritdoc cref="RepeatAsync{T}(string,Func{T},CancellationToken)"/>
    Task RepeatAsync(string message, Action? behavior, CancellationToken canceler);

    /// <inheritdoc cref="ILimiterSync.Repeat{T}(string,Func{T},CancellationToken)"/>
    Task<IReadOnlyCollection<T>> RepeatAsync<T>(
        string message,
        Func<T> behavior,
        CancellationToken canceler
    );

    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <inheritdoc cref="ILimiterSync.StallUntil(string,Func{bool},CancellationToken)"/>
    Task StallUntilAsync(string message, Func<bool> behavior, CancellationToken canceler);

    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <inheritdoc cref="StallUntilAsync{T}(string,Func{T},Func{bool},CancellationToken)"/>
    Task StallUntilAsync(
        string message,
        Action? behavior,
        Func<bool> checkState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="StallUntilAsync{T}(string,Func{T},Func{T,bool},CancellationToken)"/>
    Task<IReadOnlyCollection<T>> StallUntilAsync<T>(
        string message,
        Func<T> behavior,
        Func<bool> checkState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="ILimiterSync.StallUntil{T}(string,Func{T},Func{bool},CancellationToken)"/>
    Task<IReadOnlyCollection<T>> StallUntilAsync<T>(
        string message,
        Func<T> behavior,
        Func<T, bool> checkState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="RetryAsync{T}(string,Action,Action,CancellationToken)"/>
    Task RetryAsync(string message, Action behavior, CancellationToken canceler);

    /// <inheritdoc cref="RetryAsync{T}(string,Action,Action,CancellationToken)"/>
    Task RetryAsync(string message, Action behavior, Action resetState, CancellationToken canceler);

    /// <inheritdoc cref="RetryAsync{T}(string,Action,Action,CancellationToken)"/>
    Task RetryAsync<TError>(string message, Action behavior, CancellationToken canceler)
        where TError : Exception;

    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <inheritdoc cref="RetryAsync{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task RetryAsync<TError>(
        string message,
        Action behavior,
        Action? resetState,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="RetryAsync{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task<TResult> RetryAsync<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken canceler
    );

    /// <inheritdoc cref="RetryAsync{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task<TResult> RetryAsync<TResult>(
        string message,
        Func<TResult> behavior,
        Action resetState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="RetryAsync{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task<TResult> RetryAsync<TError, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="ILimiterSync.Retry{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task<TResult> RetryAsync<TError, TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="AttemptAsync{T}(string,Action,Action,CancellationToken)"/>
    Task AttemptAsync(string message, Action behavior, CancellationToken canceler);

    /// <inheritdoc cref="AttemptAsync{T}(string,Action,Action,CancellationToken)"/>
    Task AttemptAsync(
        string message,
        Action behavior,
        Action resetState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="AttemptAsync{T}(string,Action,Action,CancellationToken)"/>
    Task AttemptAsync<TError>(string message, Action behavior, CancellationToken canceler)
        where TError : Exception;

    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <inheritdoc cref="AttemptAsync{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task AttemptAsync<TError>(
        string message,
        Action? behavior,
        Action? resetState,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="AttemptAsync{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task<TResult?> AttemptAsync<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken canceler
    );

    /// <inheritdoc cref="AttemptAsync{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task<TResult?> AttemptAsync<TResult>(
        string message,
        Func<TResult> behavior,
        Action resetState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="AttemptAsync{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task<TResult?> AttemptAsync<TError, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="ILimiterSync.Attempt{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task<TResult?> AttemptAsync<TError, TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken canceler
    )
        where TError : Exception;
}
