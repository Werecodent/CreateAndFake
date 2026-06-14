using CreateAndFake.Design;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertErrorExtensions
{
    /// <inheritdoc cref="AssertErrorBase{T}.Fail(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task Fail<T>(this Task<T> origin, string? details = null)
        where T : AssertErrorBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        (await origin.ConfigureAwait(false)).Fail(details);
    }

    /// <inheritdoc cref="AssertErrorBase{T}.Fail(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task Fail<T>(
        this Task<T> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertErrorBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        (await origin.ConfigureAwait(false)).Fail(optionConfiguration, details);
    }
}
