namespace CreateAndFake.Design.Reiteration;

/// <summary>Provides the core functionality for asynchronous task repetition.</summary>
public interface ILimiterTask
{
    /// <inheritdoc cref="ILimiterAsync.RepeatAsync(string,Action,CancellationToken)"/>
    Task RepeatAsync(string message, Task? behavior, CancellationToken canceler);

    /// <inheritdoc cref="ILimiterAsync.RepeatAsync{T}(string,Func{T},CancellationToken)"/>
    Task<IReadOnlyCollection<T>> RepeatAsync<T>(
        string message,
        Task<T> behavior,
        CancellationToken canceler
    );

    /// <inheritdoc cref="ILimiterAsync.StallUntilAsync(string,Func{bool},CancellationToken)"/>
    Task StallUntilAsync(string message, Task<bool> behavior, CancellationToken canceler);

    /// <inheritdoc cref="ILimiterAsync.StallUntilAsync(string,Action?,Func{bool},CancellationToken)"/>
    Task StallUntilAsync(
        string message,
        Task? behavior,
        Func<bool> checkState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="ILimiterAsync.StallUntilAsync(string,Action?,Func{bool},CancellationToken)"/>
    Task StallUntilAsync(
        string message,
        Task? behavior,
        Task<bool> checkState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="StallUntilAsync{T}(string,Task{T},Func{T,bool},CancellationToken)"/>
    Task<IReadOnlyCollection<T>> StallUntilAsync<T>(
        string message,
        Task<T> behavior,
        Func<bool> checkState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="StallUntilAsync{T}(string,Task{T},Func{T,bool},CancellationToken)"/>
    Task<IReadOnlyCollection<T>> StallUntilAsync<T>(
        string message,
        Task<T> behavior,
        Task<bool> checkState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="ILimiterAsync.StallUntilAsync{T}(string,Func{T},Func{bool},CancellationToken)"/>
    Task<IReadOnlyCollection<T>> StallUntilAsync<T>(
        string message,
        Task<T> behavior,
        Func<T, bool> checkState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="RetryAsync{T}(string,Task,Action,CancellationToken)"/>
    Task RetryAsync(string message, Task behavior, CancellationToken canceler);

    /// <inheritdoc cref="RetryAsync{T}(string,Task,Action,CancellationToken)"/>
    Task RetryAsync(string message, Task behavior, Action resetState, CancellationToken canceler);

    /// <inheritdoc cref="RetryAsync{T}(string,Task,Action,CancellationToken)"/>
    Task RetryAsync(string message, Task behavior, Task resetState, CancellationToken canceler);

    /// <inheritdoc cref="RetryAsync{T}(string,Task,Action,CancellationToken)"/>
    Task RetryAsync<TError>(string message, Task behavior, CancellationToken canceler)
        where TError : Exception;

    /// <inheritdoc cref="ILimiterAsync.RetryAsync{T}(string,Action,Action,CancellationToken)"/>
    Task RetryAsync<TError>(
        string message,
        Task behavior,
        Action? resetState,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="ILimiterAsync.RetryAsync{T}(string,Action,Action,CancellationToken)"/>
    Task RetryAsync<TError>(
        string message,
        Task behavior,
        Task? resetState,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="RetryAsync{T,T}(string,Task{T},Action,CancellationToken)"/>
    Task<TResult> RetryAsync<TResult>(
        string message,
        Task<TResult> behavior,
        CancellationToken canceler
    );

    /// <inheritdoc cref="RetryAsync{T,T}(string,Task{T},Action,CancellationToken)"/>
    Task<TResult> RetryAsync<TResult>(
        string message,
        Task<TResult> behavior,
        Action resetState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="RetryAsync{T,T}(string,Task{T},Action,CancellationToken)"/>
    Task<TResult> RetryAsync<TResult>(
        string message,
        Task<TResult> behavior,
        Task resetState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="RetryAsync{T,T}(string,Task{T},Action,CancellationToken)"/>
    Task<TResult> RetryAsync<TError, TResult>(
        string message,
        Task<TResult> behavior,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="ILimiterAsync.RetryAsync{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task<TResult> RetryAsync<TError, TResult>(
        string message,
        Task<TResult> behavior,
        Action? resetState,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="ILimiterAsync.RetryAsync{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task<TResult> RetryAsync<TError, TResult>(
        string message,
        Task<TResult> behavior,
        Task? resetState,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="AttemptAsync{T}(string,Task,Action,CancellationToken)"/>
    Task AttemptAsync(string message, Task behavior, CancellationToken canceler);

    /// <inheritdoc cref="AttemptAsync{T}(string,Task,Action,CancellationToken)"/>
    Task AttemptAsync(string message, Task behavior, Action resetState, CancellationToken canceler);

    /// <inheritdoc cref="AttemptAsync{T}(string,Task,Action,CancellationToken)"/>
    Task AttemptAsync(string message, Task behavior, Task resetState, CancellationToken canceler);

    /// <inheritdoc cref="AttemptAsync{T}(string,Task,Action,CancellationToken)"/>
    Task AttemptAsync<TError>(string message, Task behavior, CancellationToken canceler)
        where TError : Exception;

    /// <inheritdoc cref="ILimiterAsync.AttemptAsync{T}(string,Action,Action,CancellationToken)"/>
    Task AttemptAsync<TError>(
        string message,
        Task? behavior,
        Action? resetState,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="ILimiterAsync.AttemptAsync{T}(string,Action,Action,CancellationToken)"/>
    Task AttemptAsync<TError>(
        string message,
        Task? behavior,
        Task? resetState,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="AttemptAsync{T,T}(string,Task{T},Action,CancellationToken)"/>
    Task<TResult?> AttemptAsync<TResult>(
        string message,
        Task<TResult> behavior,
        CancellationToken canceler
    );

    /// <inheritdoc cref="AttemptAsync{T,T}(string,Task{T},Action,CancellationToken)"/>
    Task<TResult?> AttemptAsync<TResult>(
        string message,
        Task<TResult> behavior,
        Action resetState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="AttemptAsync{T,T}(string,Task{T},Action,CancellationToken)"/>
    Task<TResult?> AttemptAsync<TResult>(
        string message,
        Task<TResult> behavior,
        Task resetState,
        CancellationToken canceler
    );

    /// <inheritdoc cref="AttemptAsync{T,T}(string,Task{T},Action,CancellationToken)"/>
    Task<TResult?> AttemptAsync<TError, TResult>(
        string message,
        Task<TResult> behavior,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="ILimiterAsync.AttemptAsync{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task<TResult?> AttemptAsync<TError, TResult>(
        string message,
        Task<TResult> behavior,
        Action? resetState,
        CancellationToken canceler
    )
        where TError : Exception;

    /// <inheritdoc cref="ILimiterAsync.AttemptAsync{T,T}(string,Func{T},Action,CancellationToken)"/>
    Task<TResult?> AttemptAsync<TError, TResult>(
        string message,
        Task<TResult> behavior,
        Task? resetState,
        CancellationToken canceler
    )
        where TError : Exception;
}
