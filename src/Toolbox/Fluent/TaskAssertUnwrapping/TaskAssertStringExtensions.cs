using CreateAndFake.Design;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent;

#pragma warning disable MA0042 // Following the pattern.

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertStringExtensions
{
    /// <inheritdoc cref="AssertStringBase{T}.Contains(string,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Contains<T>(
        this Task<T> origin,
        string content,
        string? details = null
    )
        where T : AssertStringBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Contains(content, details);
    }

    /// <inheritdoc cref="AssertStringBase{T}.Contains(string,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Contains<T>(
        this Task<T> origin,
        string content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertStringBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Contains(content, optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertStringBase{T}.ContainsNot(string,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ContainsNot<T>(
        this Task<T> origin,
        string content,
        string? details = null
    )
        where T : AssertStringBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ContainsNot(content, details);
    }

    /// <inheritdoc cref="AssertStringBase{T}.ContainsNot(string,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ContainsNot<T>(
        this Task<T> origin,
        string content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertStringBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ContainsNot(
            content,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertStringBase{T}.StartsWith(string,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> StartsWith<T>(
        this Task<T> origin,
        string content,
        string? details = null
    )
        where T : AssertStringBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).StartsWith(content, details);
    }

    /// <inheritdoc cref="AssertStringBase{T}.StartsWith(string,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> StartsWith<T>(
        this Task<T> origin,
        string content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertStringBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).StartsWith(
            content,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertStringBase{T}.StartsNotWith(string,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> StartsNotWith<T>(
        this Task<T> origin,
        string content,
        string? details = null
    )
        where T : AssertStringBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).StartsNotWith(content, details);
    }

    /// <inheritdoc cref="AssertStringBase{T}.StartsNotWith(string,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> StartsNotWith<T>(
        this Task<T> origin,
        string content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertStringBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).StartsNotWith(
            content,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertStringBase{T}.EndsWith(string,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> EndsWith<T>(
        this Task<T> origin,
        string content,
        string? details = null
    )
        where T : AssertStringBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).EndsWith(content, details);
    }

    /// <inheritdoc cref="AssertStringBase{T}.EndsWith(string,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> EndsWith<T>(
        this Task<T> origin,
        string content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertStringBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).EndsWith(content, optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertStringBase{T}.EndsNotWith(string,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> EndsNotWith<T>(
        this Task<T> origin,
        string content,
        string? details = null
    )
        where T : AssertStringBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).EndsNotWith(content, details);
    }

    /// <inheritdoc cref="AssertStringBase{T}.EndsNotWith(string,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> EndsNotWith<T>(
        this Task<T> origin,
        string content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertStringBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).EndsNotWith(
            content,
            optionConfiguration,
            details
        );
    }
}

#pragma warning restore MA0042
