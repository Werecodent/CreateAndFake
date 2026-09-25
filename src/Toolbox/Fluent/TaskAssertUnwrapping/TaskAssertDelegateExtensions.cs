using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

#pragma warning disable MA0042 // Following the pattern.

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

    /// <inheritdoc cref="AssertDelegateBase{T}.ThrowsException(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ExceptionChainer<Exception>> ThrowsException<T>(
        this Task<T> origin,
        string? details = null
    )
        where T : AssertDelegateBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ThrowsException(details);
    }

    /// <inheritdoc cref="AssertDelegateBase{T}.ThrowsException(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ExceptionChainer<Exception>> ThrowsException<T>(
        this Task<T> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertDelegateBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ThrowsException(optionConfiguration, details);
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

    /// <inheritdoc cref="AssertDelegateBase{T}.ThrowsNoException(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AlsoChainer> ThrowsNoException<T>(
        this Task<T> origin,
        string? details = null
    )
        where T : AssertDelegateBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ThrowsNoException(details);
    }

    /// <inheritdoc cref="AssertDelegateBase{T}.ThrowsNoException(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AlsoChainer> ThrowsNoException<T>(
        this Task<T> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertDelegateBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ThrowsNoException(optionConfiguration, details);
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

    /// <inheritdoc cref="AssertDelegateBase{T}.HasResult{T}(T,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ResultChainer<TItem>> HasResult<T, TItem>(
        this Task<T> origin,
        TItem expected,
        string? details = null
    )
        where T : AssertDelegateBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasResult(expected, details);
    }

    /// <inheritdoc cref="AssertDelegateBase{T}.HasResult{T}(T,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ResultChainer<TItem>> HasResult<T, TItem>(
        this Task<T> origin,
        TItem expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertDelegateBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasResult(
            expected,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertDelegateBase{T}.HasResultAsync{T}(T,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ResultChainer<TItem>> HasResultAsync<T, TItem>(
        this Task<T> origin,
        TItem expected,
        CancellationToken canceler,
        string? details = null
    )
        where T : AssertDelegateBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasResultAsync(expected, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertDelegateBase{T}.HasResultAsync{T}(T,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<ResultChainer<TItem>> HasResultAsync<T, TItem>(
        this Task<T> origin,
        TItem expected,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertDelegateBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasResultAsync(expected, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }
}

#pragma warning restore
