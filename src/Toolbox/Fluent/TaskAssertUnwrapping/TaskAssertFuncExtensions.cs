using CreateAndFake.Design;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertFuncExtensions
{
    /// <inheritdoc cref="AssertFuncBase{T,T}.HasResult(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ResultChainer<T>> HasResult<T>(
        this Task<AssertFunc<T>> origin,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasResult(details);
    }

    /// <inheritdoc cref="AssertFuncBase{T,T}.HasResult(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ResultChainer<T>> HasResult<T>(
        this Task<AssertFunc<T>> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasResult(optionConfiguration, details);
    }
}
