using CreateAndFake.Design;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;

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

    /// <inheritdoc cref="AssertErrorBase{T}.HasInner{T}(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ExceptionChainer<TException>> HasInner<TException>(
        this Task<AssertError> origin,
        string? details = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasInner<TException>(details);
    }

    /// <inheritdoc cref="AssertErrorBase{T}.HasInner{T}(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ExceptionChainer<TException>> HasInner<TException>(
        this Task<AssertError> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasInner<TException>(
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertErrorBase{T}.HasInner{T}(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> HasInnerException<T>(
        this Task<T> origin,
        Exception? inner,
        string? details = null
    )
        where T : AssertErrorBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasInnerException(inner, details);
    }

    /// <inheritdoc cref="AssertErrorBase{T}.HasInner{T}(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> HasInnerException<T>(
        this Task<T> origin,
        Exception? inner,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertErrorBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasInnerException(
            inner,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertErrorBase{T}.Debug(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Debug<T>(this Task<T> origin, string? details = null)
        where T : AssertErrorBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Debug(details);
    }

    /// <inheritdoc cref="AssertErrorBase{T}.Debug(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Debug<T>(
        this Task<T> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertErrorBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Debug(optionConfiguration, details);
    }
}
