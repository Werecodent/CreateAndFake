using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertActionExtensions
{
    /// <inheritdoc cref="AssertActionBase{T}.Throws{T}(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ExceptionChainer<TException>> Throws<TException>(
        this Task<AssertAction> origin,
        string? details = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Throws<TException>(details);
    }

    /// <inheritdoc cref="AssertActionBase{T}.Throws{T}(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ExceptionChainer<TException>> Throws<TException>(
        this Task<AssertAction> origin,
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

    /// <inheritdoc cref="AssertActionBase{T}.ThrowsNo{T}(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AlsoChainer> ThrowsNo<TException>(
        this Task<AssertAction> origin,
        string? details = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ThrowsNo<TException>(details);
    }

    /// <inheritdoc cref="AssertActionBase{T}.ThrowsNo{T}(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AlsoChainer> ThrowsNo<TException>(
        this Task<AssertAction> origin,
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
}
