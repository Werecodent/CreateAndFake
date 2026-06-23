using CreateAndFake.Design;
using CreateAndFake.Fluent.AssertAsyncCalls;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertGenericValueTaskExtensions
{
    /// <inheritdoc cref="AssertGenericValueTaskBase{T,T}.HasResultAsync(CancellationToken,string)"/>
    /// <returns><inheritdoc cref="ResultChainer{T}" path="/summary"/></returns>
    public static async Task<ResultChainer<T>> HasResultAsync<T>(
        this Task<AssertGenericValueTask<T>> origin,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasResultAsync(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertGenericValueTaskBase{T,T}.HasResultAsync(CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="ResultChainer{T}" path="/summary"/></returns>
    public static async Task<ResultChainer<T>> HasResultAsync<T>(
        this Task<AssertGenericValueTask<T>> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasResultAsync(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertGenericValueTaskBase{T,T}.ThrowsAsync{T}(CancellationToken,string)"/>
    /// <returns><inheritdoc cref="ExceptionChainer{T}" path="/summary"/></returns>
    public static async Task<ExceptionChainer<Exception>> ThrowsExceptionAsync<T>(
        this Task<AssertGenericValueTask<T>> origin,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsAsync<Exception>(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertGenericValueTaskBase{T,T}.ThrowsAsync{T}(CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="ExceptionChainer{T}" path="/summary"/></returns>
    public static async Task<ExceptionChainer<Exception>> ThrowsExceptionAsync<T>(
        this Task<AssertGenericValueTask<T>> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsAsync<Exception>(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertGenericValueTaskBase{T,T}.ThrowsNoAsync{T}(CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public static async Task<AlsoChainer> ThrowsNoExceptionAsync<T>(
        this Task<AssertGenericValueTask<T>> origin,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsNoAsync<Exception>(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertGenericValueTaskBase{T,T}.ThrowsNoAsync{T}(CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public static async Task<AlsoChainer> ThrowsNoExceptionAsync<T>(
        this Task<AssertGenericValueTask<T>> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsNoAsync<Exception>(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }
}
