using CreateAndFake.Design;
using CreateAndFake.Fluent.AssertAsyncCalls;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertValueTaskExtensions
{
    /// <inheritdoc cref="AssertValueTaskBase{T}.ThrowsAsync{T}(CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ExceptionChainer<TException>> ThrowsAsync<TException>(
        this Task<AssertValueTask> origin,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsAsync<TException>(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertValueTaskBase{T}.ThrowsAsync{T}(CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ExceptionChainer<TException>> ThrowsAsync<TException>(
        this Task<AssertValueTask> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsAsync<TException>(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertValueTaskBase{T}.ThrowsNoAsync{T}(CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AlsoChainer> ThrowsNoAsync<TException>(
        this Task<AssertValueTask> origin,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsNoAsync<TException>(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertValueTaskBase{T}.ThrowsNoAsync{T}(CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AlsoChainer> ThrowsNoAsync<TException>(
        this Task<AssertValueTask> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ThrowsNoAsync<TException>(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }
}
