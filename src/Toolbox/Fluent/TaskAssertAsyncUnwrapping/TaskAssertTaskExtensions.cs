using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertTaskExtensions
{
    /// <inheritdoc cref="AssertTaskBase{T}.ThrowsAsync{T}(CancellationToken,string)"/>
    /// <returns><inheritdoc cref="ExceptionChainer{T}" path="/summary"/></returns>
    public static async Task<ExceptionChainer<T>> ThrowsAsync<T>(
        this Task<AssertTask> origin,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsAsync<T>(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertTaskBase{T}.ThrowsAsync{T}(CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="ExceptionChainer{T}" path="/summary"/></returns>
    public static async Task<ExceptionChainer<T>> ThrowsAsync<T>(
        this Task<AssertTask> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsAsync<T>(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertTaskBase{T}.ThrowsAsync{T}(CancellationToken,string)"/>
    /// <returns><inheritdoc cref="ExceptionChainer{T}" path="/summary"/></returns>
    public static async Task<ExceptionChainer<Exception>> ThrowsExceptionAsync<T>(
        this Task<T> origin,
        CancellationToken canceler,
        string? details = null
    )
        where T : AssertTaskBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsExceptionAsync(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertTaskBase{T}.ThrowsExceptionAsync(CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="ExceptionChainer{T}" path="/summary"/></returns>
    public static async Task<ExceptionChainer<Exception>> ThrowsExceptionAsync<T>(
        this Task<T> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertTaskBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsExceptionAsync(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertTaskBase{T}.ThrowsNoAsync{T}(CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public static async Task<AlsoChainer> ThrowsNoAsync<T>(
        this Task<AssertTask> origin,
        CancellationToken canceler,
        string? details = null
    )
        where T : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsNoAsync<T>(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertTaskBase{T}.ThrowsNoAsync{T}(CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public static async Task<AlsoChainer> ThrowsNoAsync<T>(
        this Task<AssertTask> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsNoAsync<T>(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertTaskBase{T}.ThrowsNoExceptionAsync(CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public static async Task<AlsoChainer> ThrowsNoExceptionAsync<T>(
        this Task<T> origin,
        CancellationToken canceler,
        string? details = null
    )
        where T : AssertTaskBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsNoExceptionAsync(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertTaskBase{T}.ThrowsNoExceptionAsync(CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public static async Task<AlsoChainer> ThrowsNoExceptionAsync<T>(
        this Task<T> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertTaskBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsNoExceptionAsync(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }
}
