namespace CreateAndFake.Design.Reiteration;

/// <summary>Provides the core functionality for synchronous repetition.</summary>
public interface ISyncLimiter
{
    /// <inheritdoc cref="Repeat{TResult}(string,Func{TResult},CancellationToken?)"/>
    void Repeat(string message, Action? behavior, CancellationToken? canceler = null);

    /// <summary>Repeats <paramref name="behavior"/> until the limit is reached.</summary>
    /// <returns>Results from all calls.</returns>
    /// <inheritdoc cref="Attempt{TError,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    IReadOnlyCollection<TResult> Repeat<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    );

    /// <summary>Retries <paramref name="behavior"/> until it returns <see langword="true"/>.</summary>
    /// <inheritdoc cref="StallUntil(string,Action,Func{bool},CancellationToken?)"/>
    void StallUntil(string message, Func<bool> behavior, CancellationToken? canceler = null);

    /// <returns></returns>
    /// <inheritdoc cref="StallUntil{T}(string,Func{T},Func{bool},CancellationToken?)"/>
    void StallUntil(
        string message,
        Action? behavior,
        Func<bool> checkState,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="StallUntil{T}(string,Func{T},Func{T,bool},CancellationToken?)"/>
    IReadOnlyCollection<TResult> StallUntil<TResult>(
        string message,
        Func<TResult> behavior,
        Func<bool> checkState,
        CancellationToken? canceler = null
    );

    /// <summary>Retries <paramref name="behavior"/> until <paramref name="checkState"/>.</summary>
    /// <param name="checkState">Polls if the behavior was successful.</param>
    /// <returns>Results from all calls.</returns>
    /// <inheritdoc cref="Attempt{TError,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    IReadOnlyCollection<TResult> StallUntil<TResult>(
        string message,
        Func<TResult> behavior,
        Func<TResult, bool> checkState,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="Retry{TError}(string,Action,Action?,CancellationToken?)"/>
    void Retry(string message, Action behavior, CancellationToken? canceler = null);

    /// <inheritdoc cref="Retry{TError}(string,Action,Action?,CancellationToken?)"/>
    void Retry(
        string message,
        Action behavior,
        Action resetState,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="Retry{TError}(string,Action,Action?,CancellationToken?)"/>
    void Retry<TError>(string message, Action behavior, CancellationToken? canceler = null)
        where TError : Exception;

    /// <returns></returns>
    /// <inheritdoc cref="Retry{TError,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    void Retry<TError>(
        string message,
        Action behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception;

    /// <inheritdoc cref="Retry{TError,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    TResult Retry<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="Retry{TError,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    TResult Retry<TResult>(
        string message,
        Func<TResult> behavior,
        Action resetState,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="Retry{TError,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    TResult Retry<TError, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
        where TError : Exception;

    /// <returns>Result of the successful <paramref name="behavior"/> attempt.</returns>
    /// <exception cref="TimeoutException">If the limit is reached.</exception>
    /// <inheritdoc cref="Attempt{TError,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    TResult Retry<TError, TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception;

    /// <inheritdoc cref="Attempt{TError}(string,Action?,Action,CancellationToken?)"/>
    void Attempt(string message, Action behavior, CancellationToken? canceler = null);

    /// <inheritdoc cref="Attempt{TError}(string,Action?,Action,CancellationToken?)"/>
    void Attempt(
        string message,
        Action behavior,
        Action resetState,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="Attempt{TError}(string,Action?,Action,CancellationToken?)"/>
    void Attempt<TError>(string message, Action behavior, CancellationToken? canceler = null)
        where TError : Exception;

    /// <returns></returns>
    /// <inheritdoc cref="Attempt{TError,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    void Attempt<TError>(
        string message,
        Action? behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception;

    /// <inheritdoc cref="Attempt{TError,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    TResult? Attempt<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="Attempt{TError,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    TResult? Attempt<TResult>(
        string message,
        Func<TResult> behavior,
        Action resetState,
        CancellationToken? canceler = null
    );

    /// <inheritdoc cref="Attempt{TError,TResult}(string,Func{TResult},Action,CancellationToken?)"/>
    TResult? Attempt<TError, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken? canceler = null
    )
        where TError : Exception;

    /// <summary>Retries <paramref name="behavior"/> upon encountering exceptions.</summary>
    /// <typeparam name="TError">
    ///     Exception <c>Type</c> to ignore and retry <paramref name="behavior"/> if encountered.
    /// </typeparam>
    /// <typeparam name="TResult">
    ///     Result <c>Type</c> returned from <paramref name="behavior"/>.
    /// </typeparam>
    /// <param name="message">Details to include when throwing exceptions.</param>
    /// <param name="behavior">Code to repeatably attempt.</param>
    /// <param name="resetState">Code to run between attempts.</param>
    /// <param name="canceler">Token indicating attempts should be canceled.</param>
    /// <returns>
    ///     Result of the successful <paramref name="behavior"/>
    ///     attempt or <see langword="default"/> if limit reached.
    /// </returns>
    /// <exception cref="TimeoutException">If cancelled via <paramref name="canceler"/>.</exception>
    /// <remarks>Beware infinite loops in <paramref name="behavior"/>.</remarks>
    TResult? Attempt<TError, TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken? canceler = null
    )
        where TError : Exception;
}
