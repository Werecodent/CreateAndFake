using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertDelegateExtensions
{
    /// <inheritdoc cref="AssertDelegateBase{T}.Throws{T}(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ExceptionChainer<TException>> Throws<TException>(
        this Task<AssertDelegate> origin,
        string? details = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Throws<TException>(details);
    }

    /// <inheritdoc cref="AssertDelegateBase{T}.Throws{T}(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ExceptionChainer<TException>> Throws<TException>(
        this Task<AssertDelegate> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Throws<TException>(
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertDelegateBase{T}.ThrowsNo{T}(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AlsoChainer> ThrowsNo<TException>(
        this Task<AssertDelegate> origin,
        string? details = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ThrowsNo<TException>(details);
    }

    /// <inheritdoc cref="AssertDelegateBase{T}.ThrowsNo{T}(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AlsoChainer> ThrowsNo<TException>(
        this Task<AssertDelegate> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ThrowsNo<TException>(
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertDelegateBase{T}.HasResult{T}(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ResultChainer<TResult>> HasResult<TResult>(
        this Task<AssertDelegate> origin,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasResult<TResult>(details);
    }

    /// <inheritdoc cref="AssertDelegateBase{T}.HasResult{T}(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ResultChainer<TResult>> HasResult<TResult>(
        this Task<AssertDelegate> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasResult<TResult>(
            optionConfiguration,
            details
        );
    }
}
