using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertTaskExtensions
{
    /// <inheritdoc cref="AssertGenericValueTaskBase{T,T}.ThrowsAsync{T}(CancellationToken,string)"/>
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
            .ThrowsAsync<Exception>(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertGenericValueTaskBase{T,T}.ThrowsAsync{T}(CancellationToken,AsserterMod,string)"/>
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
            .ThrowsAsync<Exception>(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertGenericValueTaskBase{T,T}.ThrowsNoAsync{T}(CancellationToken,string)"/>
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
            .ThrowsNoAsync<Exception>(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertGenericValueTaskBase{T,T}.ThrowsNoAsync{T}(CancellationToken,AsserterMod,string)"/>
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
            .ThrowsNoAsync<Exception>(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }
}
