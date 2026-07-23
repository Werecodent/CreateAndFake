using System.Diagnostics;

namespace CreateAndFake.Design.Reiteration;

/// <inheritdoc cref="ILimiter"/>
public sealed partial class Limiter : ILimiterTask
{
    /// <inheritdoc/>
    public Task RepeatAsync(string message, Func<Task?> behavior, CancellationToken canceler)
    {
        return RepeatAsync(message, () => ToGenericAsync(behavior), canceler);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<T>> RepeatAsync<T>(
        string message,
        Func<Task<T>> behavior,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(behavior);

        int i = 0;
        List<T> results = [];
        Stopwatch watch = Stopwatch.StartNew();
        do
        {
            results.Add(await SafeInvokeAsync(behavior).ConfigureAwait(false));
        } while (
            await DelayIfNotDoneAsync(message, watch.Elapsed, ++i, canceler).ConfigureAwait(false)
        );

        return results.AsReadOnly();
    }

    /// <inheritdoc/>
    public Task StallUntilAsync(
        string message,
        Func<Task<bool>> behavior,
        CancellationToken canceler
    )
    {
        return StallUntilAsync(message, () => Task.CompletedTask, behavior, canceler);
    }

    /// <inheritdoc/>
    public Task StallUntilAsync(
        string message,
        Func<Task?> behavior,
        Func<bool> checkState,
        CancellationToken canceler
    )
    {
        return StallUntilAsync(message, () => ToGenericAsync(behavior), checkState, canceler);
    }

    /// <inheritdoc/>
    public Task StallUntilAsync(
        string message,
        Func<Task?> behavior,
        Func<Task<bool>> checkState,
        CancellationToken canceler
    )
    {
        return StallUntilAsync(message, () => ToGenericAsync(behavior), checkState, canceler);
    }

    /// <inheritdoc/>
    public Task<IReadOnlyCollection<T>> StallUntilAsync<T>(
        string message,
        Func<Task<T>> behavior,
        Func<bool> checkState,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(checkState);
        return StallUntilAsync(message, behavior, (T _) => checkState.Invoke(), canceler);
    }

    /// <inheritdoc/>
    public Task<IReadOnlyCollection<T>> StallUntilAsync<T>(
        string message,
        Func<Task<T>> behavior,
        Func<Task<bool>> checkState,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(checkState);
        return StallUntilAsync(message, behavior, _ => checkState.Invoke(), canceler);
    }

    /// <inheritdoc/>
    public Task<IReadOnlyCollection<T>> StallUntilAsync<T>(
        string message,
        Func<Task<T>> behavior,
        Func<T, bool> checkState,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(checkState);
        return StallUntilAsync(
            message,
            behavior,
            x => Task.FromResult(checkState.Invoke(x)),
            canceler
        );
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<T>> StallUntilAsync<T>(
        string message,
        Func<Task<T>> behavior,
        Func<T, Task<bool>> checkState,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(behavior, checkState);

        List<T> results = [];
        Stopwatch watch = Stopwatch.StartNew();
        for (int i = 1; true; i++)
        {
            T result = await SafeInvokeAsync(behavior).ConfigureAwait(false);

            results.Add(result);

            Task<bool> checker = checkState.Invoke(result);
            if (await SafeInvokeAsync(() => checker).ConfigureAwait(false))
            {
                break;
            }
            await DelayOrFaultAsync(message, watch.Elapsed, i, canceler).ConfigureAwait(false);
        }

        return results.AsReadOnly();
    }

    /// <inheritdoc/>
    public Task RetryAsync(string message, Func<Task> behavior, CancellationToken canceler)
    {
        return RetryAsync<Exception>(message, behavior, (Action?)null, canceler);
    }

    /// <inheritdoc/>
    public Task RetryAsync(
        string message,
        Func<Task> behavior,
        Action resetState,
        CancellationToken canceler
    )
    {
        return RetryAsync<Exception>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public Task RetryAsync(
        string message,
        Func<Task> behavior,
        Func<Task?> resetState,
        CancellationToken canceler
    )
    {
        return RetryAsync<Exception>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public Task RetryAsync<TError>(string message, Func<Task> behavior, CancellationToken canceler)
        where TError : Exception
    {
        return RetryAsync<TError>(message, behavior, (Action?)null, canceler);
    }

    /// <inheritdoc/>
    public Task RetryAsync<TError>(
        string message,
        Func<Task> behavior,
        Action? resetState,
        CancellationToken canceler
    )
        where TError : Exception
    {
        return RetryAsync<TError, bool>(
            message,
            () => ToGenericAsync(behavior),
            resetState,
            canceler
        );
    }

    /// <inheritdoc/>
    public Task RetryAsync<TError>(
        string message,
        Func<Task> behavior,
        Func<Task?>? resetState,
        CancellationToken canceler
    )
        where TError : Exception
    {
        return RetryAsync<TError, bool>(
            message,
            () => ToGenericAsync(behavior),
            resetState,
            canceler
        );
    }

    /// <inheritdoc/>
    public Task<TResult> RetryAsync<TResult>(
        string message,
        Func<Task<TResult>> behavior,
        CancellationToken canceler
    )
    {
        return RetryAsync<Exception, TResult>(message, behavior, (Action?)null, canceler);
    }

    /// <inheritdoc/>
    public Task<TResult> RetryAsync<TResult>(
        string message,
        Func<Task<TResult>> behavior,
        Action resetState,
        CancellationToken canceler
    )
    {
        return RetryAsync<Exception, TResult>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public Task<TResult> RetryAsync<TResult>(
        string message,
        Func<Task<TResult>> behavior,
        Func<Task?> resetState,
        CancellationToken canceler
    )
    {
        return RetryAsync<Exception, TResult>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public Task<TResult> RetryAsync<TError, TResult>(
        string message,
        Func<Task<TResult>> behavior,
        CancellationToken canceler
    )
        where TError : Exception
    {
        return RetryAsync<TError, TResult>(message, behavior, (Action?)null, canceler);
    }

    /// <inheritdoc/>
    public async Task<TResult> RetryAsync<TError, TResult>(
        string message,
        Func<Task<TResult>> behavior,
        Action? resetState,
        CancellationToken canceler
    )
        where TError : Exception
    {
        ArgumentGuard.ThrowIfNull(behavior);

        Stopwatch watch = Stopwatch.StartNew();
        for (int i = 1; true; i++)
        {
            TError lastError;
            try
            {
                return await SafeInvokeAsync(behavior).ConfigureAwait(false);
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
    public async Task<TResult> RetryAsync<TError, TResult>(
        string message,
        Func<Task<TResult>> behavior,
        Func<Task?>? resetState,
        CancellationToken canceler
    )
        where TError : Exception
    {
        ArgumentGuard.ThrowIfNull(behavior);

        Stopwatch watch = Stopwatch.StartNew();
        for (int i = 1; true; i++)
        {
            TError lastError;
            try
            {
                return await SafeInvokeAsync(behavior).ConfigureAwait(false);
            }
            catch (TError error)
            {
                lastError = error;
            }

            Task? resetter = resetState?.Invoke();
            if (resetter != null)
            {
                await resetter.ConfigureAwait(false);
            }

            await DelayOrFaultAsync(message, watch.Elapsed, i, canceler, lastError)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public Task AttemptAsync(string message, Func<Task> behavior, CancellationToken canceler)
    {
        return AttemptAsync<Exception>(message, behavior, (Action?)null, canceler);
    }

    /// <inheritdoc/>
    public Task AttemptAsync(
        string message,
        Func<Task> behavior,
        Action resetState,
        CancellationToken canceler
    )
    {
        return AttemptAsync<Exception>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public Task AttemptAsync(
        string message,
        Func<Task> behavior,
        Func<Task?> resetState,
        CancellationToken canceler
    )
    {
        return AttemptAsync<Exception>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public Task AttemptAsync<TError>(
        string message,
        Func<Task> behavior,
        CancellationToken canceler
    )
        where TError : Exception
    {
        return AttemptAsync<TError>(message, behavior, (Action?)null, canceler);
    }

    /// <inheritdoc/>
    public Task AttemptAsync<TError>(
        string message,
        Func<Task?> behavior,
        Action? resetState,
        CancellationToken canceler
    )
        where TError : Exception
    {
        return AttemptAsync<TError, bool>(
            message,
            () => ToGenericAsync(behavior),
            resetState,
            canceler
        );
    }

    /// <inheritdoc/>
    public Task AttemptAsync<TError>(
        string message,
        Func<Task?> behavior,
        Func<Task?>? resetState,
        CancellationToken canceler
    )
        where TError : Exception
    {
        return AttemptAsync<TError, bool>(
            message,
            () => ToGenericAsync(behavior),
            resetState,
            canceler
        );
    }

    /// <inheritdoc/>
    public Task<TResult?> AttemptAsync<TResult>(
        string message,
        Func<Task<TResult>> behavior,
        CancellationToken canceler
    )
    {
        return AttemptAsync<Exception, TResult>(message, behavior, (Action?)null, canceler);
    }

    /// <inheritdoc/>
    public Task<TResult?> AttemptAsync<TResult>(
        string message,
        Func<Task<TResult>> behavior,
        Action resetState,
        CancellationToken canceler
    )
    {
        return AttemptAsync<Exception, TResult>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public Task<TResult?> AttemptAsync<TResult>(
        string message,
        Func<Task<TResult>> behavior,
        Func<Task?> resetState,
        CancellationToken canceler
    )
    {
        return AttemptAsync<Exception, TResult>(message, behavior, resetState, canceler);
    }

    /// <inheritdoc/>
    public Task<TResult?> AttemptAsync<TError, TResult>(
        string message,
        Func<Task<TResult>> behavior,
        CancellationToken canceler
    )
        where TError : Exception
    {
        return AttemptAsync<TError, TResult>(message, behavior, (Action?)null, canceler);
    }

    /// <inheritdoc/>
    public async Task<TResult?> AttemptAsync<TError, TResult>(
        string message,
        Func<Task<TResult>> behavior,
        Action? resetState,
        CancellationToken canceler
    )
        where TError : Exception
    {
        ArgumentGuard.ThrowIfNull(behavior);

        int i = 0;
        Stopwatch watch = Stopwatch.StartNew();
        do
        {
            try
            {
                return await SafeInvokeAsync(behavior).ConfigureAwait(false);
            }
            catch (TError) { }

            resetState?.Invoke();
        } while (
            await DelayIfNotDoneAsync(message, watch.Elapsed, ++i, canceler).ConfigureAwait(false)
        );

        return default;
    }

    /// <inheritdoc/>
    public async Task<TResult?> AttemptAsync<TError, TResult>(
        string message,
        Func<Task<TResult>> behavior,
        Func<Task?>? resetState,
        CancellationToken canceler
    )
        where TError : Exception
    {
        ArgumentGuard.ThrowIfNull(behavior);

        int i = 0;
        Stopwatch watch = Stopwatch.StartNew();
        do
        {
            try
            {
                return await SafeInvokeAsync(behavior).ConfigureAwait(false);
            }
            catch (TError) { }

            Task? resetter = resetState?.Invoke();
            if (resetter != null)
            {
                await resetter.ConfigureAwait(false);
            }
        } while (
            await DelayIfNotDoneAsync(message, watch.Elapsed, ++i, canceler).ConfigureAwait(false)
        );

        return default;
    }

    private static Task<T> SafeInvokeAsync<T>(Func<Task<T>> behavior)
    {
        return behavior.Invoke() ?? throw new ArgumentNullException(nameof(behavior));
    }

    private static async Task<bool> ToGenericAsync(Func<Task?> behavior)
    {
        Task? innerBehavior = behavior?.Invoke();
        if (innerBehavior != null)
        {
            await innerBehavior.ConfigureAwait(false);
        }
        return true;
    }
}
