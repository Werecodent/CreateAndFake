namespace CreateAndFake.Design.Reiteration;

/// <summary>Provides the core functionality for synchronous repetition.</summary>
public interface ISyncLimiter
{
    /// <summary>Repeats <paramref name="behavior"/> until the limit is reached.</summary>
    /// <inheritdoc cref="Attempt(string,Action,Action,CancellationToken)"/>
    void Repeat(string message, Action? behavior, CancellationToken canceler = default);

    /// <summary>Repeats <paramref name="behavior"/> until the limit is reached.</summary>
    /// <returns>Results from all calls.</returns>
    /// <inheritdoc cref="Attempt{TError,TResult}(string,Func{TResult},Action,CancellationToken)"/>
    IReadOnlyCollection<TResult> Repeat<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken canceler = default
    );

    /// <summary>
    ///     Retries the <paramref name="behavior"/> until it returns <see langword="true"/>.
    /// </summary>
    /// <inheritdoc cref="StallUntil(string,Action,Func{bool},CancellationToken)"/>
    void StallUntil(string message, Func<bool> behavior, CancellationToken canceler = default);

    /// <returns></returns>
    /// <inheritdoc cref="StallUntil{T}(string,Func{T},Func{bool},CancellationToken)"/>
    void StallUntil(
        string message,
        Action? behavior,
        Func<bool> checkState,
        CancellationToken canceler = default
    );

    /// <inheritdoc cref="StallUntil{T}(string,Func{T},Func{T,bool},CancellationToken)"/>
    IReadOnlyCollection<TResult> StallUntil<TResult>(
        string message,
        Func<TResult> behavior,
        Func<bool> checkState,
        CancellationToken canceler = default
    );

    /// <summary>
    ///     Retries <paramref name="behavior"/> until the
    ///     <paramref name="checkState"/> result is <see langword="true"/>.
    /// </summary>
    /// <param name="checkState">Polls if the behavior was successful.</param>
    /// <returns>
    ///     Results of all calls from <paramref name="behavior"/>, with the last one
    ///     being when <paramref name="checkState"/> returned <see langword="true"/>.
    /// </returns>
    /// <inheritdoc cref="Retry{TError,TResult}(string,Func{TResult},Action,CancellationToken)"/>
    IReadOnlyCollection<TResult> StallUntil<TResult>(
        string message,
        Func<TResult> behavior,
        Func<TResult, bool> checkState,
        CancellationToken canceler = default
    );

    /// <inheritdoc cref="Retry{TError}(string,Action,Action?,CancellationToken)"/>
    void Retry(string message, Action behavior, CancellationToken canceler = default);

    /// <inheritdoc cref="Retry{TError}(string,Action,Action?,CancellationToken)"/>
    void Retry(
        string message,
        Action behavior,
        Action resetState,
        CancellationToken canceler = default
    );

    /// <inheritdoc cref="Retry{TError}(string,Action,Action?,CancellationToken)"/>
    void Retry<TError>(string message, Action behavior, CancellationToken canceler = default)
        where TError : Exception;

    /// <returns></returns>
    /// <inheritdoc cref="Retry{TError,TResult}(string,Func{TResult},Action,CancellationToken)"/>
    void Retry<TError>(
        string message,
        Action behavior,
        Action? resetState,
        CancellationToken canceler = default
    )
        where TError : Exception;

    /// <inheritdoc cref="Retry{TError,TResult}(string,Func{TResult},Action,CancellationToken)"/>
    TResult Retry<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken canceler = default
    );

    /// <inheritdoc cref="Retry{TError,TResult}(string,Func{TResult},Action,CancellationToken)"/>
    TResult Retry<TResult>(
        string message,
        Func<TResult> behavior,
        Action resetState,
        CancellationToken canceler = default
    );

    /// <inheritdoc cref="Retry{TError,TResult}(string,Func{TResult},Action,CancellationToken)"/>
    TResult Retry<TError, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken canceler = default
    )
        where TError : Exception;

    /// <returns>The result of the successful <paramref name="behavior"/> attempt.</returns>
    /// <exception cref="TimeoutException">If the limit is reached.</exception>
    /// <inheritdoc cref="Attempt{TError,TResult}(string,Func{TResult},Action,CancellationToken)"/>
    TResult Retry<TError, TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken canceler = default
    )
        where TError : Exception;

    /// <inheritdoc cref="Attempt{TError}(string,Action?,Action,CancellationToken)"/>
    void Attempt(string message, Action behavior, CancellationToken canceler = default);

    /// <inheritdoc cref="Attempt{TError}(string,Action?,Action,CancellationToken)"/>
    void Attempt(
        string message,
        Action behavior,
        Action resetState,
        CancellationToken canceler = default
    );

    /// <inheritdoc cref="Attempt{TError}(string,Action?,Action,CancellationToken)"/>
    void Attempt<TError>(string message, Action behavior, CancellationToken canceler = default)
        where TError : Exception;

    /// <returns></returns>
    /// <inheritdoc cref="Attempt{TError,TResult}(string,Func{TResult},Action,CancellationToken)"/>
    void Attempt<TError>(
        string message,
        Action? behavior,
        Action? resetState,
        CancellationToken canceler = default
    )
        where TError : Exception;

    /// <inheritdoc cref="Attempt{TResult}(string,Func{TResult},Action,CancellationToken)"/>
    TResult? Attempt<TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken canceler = default
    );

    /// <inheritdoc cref="Attempt{TError,TResult}(string,Func{TResult},Action,CancellationToken)"/>
    TResult? Attempt<TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken canceler = default
    );

    /// <inheritdoc cref="Attempt{TError,TResult}(string,Func{TResult},Action,CancellationToken)"/>
    TResult? Attempt<TError, TResult>(
        string message,
        Func<TResult> behavior,
        CancellationToken canceler = default
    )
        where TError : Exception;

    /// <summary>Retries <paramref name="behavior"/> upon encountering exceptions.</summary>
    /// <typeparam name="TError">
    ///     Exception <see cref="Type"/> to ignore and retry
    ///     the <paramref name="behavior"/> if encountered.
    /// </typeparam>
    /// <typeparam name="TResult">
    ///     Result <see cref="Type"/> returned from the <paramref name="behavior"/>.
    /// </typeparam>
    /// <param name="message">Details to include when throwing exceptions.</param>
    /// <param name="behavior">Code to repeatably attempt.</param>
    /// <param name="resetState">Code to run between attempts.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>
    ///     Result of the successful <paramref name="behavior"/>
    ///     attempt or <see langword="default"/> if limit reached.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    ///     If canceled via <paramref name="canceler"/>.
    /// </exception>
    /// <remarks>Beware infinite loops in <paramref name="behavior"/>.</remarks>
    TResult? Attempt<TError, TResult>(
        string message,
        Func<TResult> behavior,
        Action? resetState,
        CancellationToken canceler = default
    )
        where TError : Exception;
}
