using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

#pragma warning disable MA0042 // Following the pattern.

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertEnumerableExtensions
{
    /// <inheritdoc cref="AssertEnumerableBase{T}.IsEmpty(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsEmpty<T>(
        this Task<T> origin,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).IsEmpty(details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.IsEmpty(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsEmpty<T>(
        this Task<T> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).IsEmpty(optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.IsNotEmpty(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsNotEmpty<T>(
        this Task<T> origin,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).IsNotEmpty(details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.IsNotEmpty(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsNotEmpty<T>(
        this Task<T> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).IsNotEmpty(optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.HasCount(int,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> HasCount<T>(
        this Task<T> origin,
        int count,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasCount(count, details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.HasCount(int,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> HasCount<T>(
        this Task<T> origin,
        int count,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasCount(count, optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.HasCountLessThan(int,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> HasCountLessThan<T>(
        this Task<T> origin,
        int count,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasCountLessThan(count, details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.HasCountLessThan(int,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> HasCountLessThan<T>(
        this Task<T> origin,
        int count,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasCountLessThan(
            count,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.HasCountLessOrExactly(int,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> HasCountLessOrExactly<T>(
        this Task<T> origin,
        int count,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasCountLessOrExactly(count, details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.HasCountLessOrExactly(int,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> HasCountLessOrExactly<T>(
        this Task<T> origin,
        int count,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasCountLessOrExactly(
            count,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.HasCountMoreThan(int,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> HasCountMoreThan<T>(
        this Task<T> origin,
        int count,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasCountMoreThan(count, details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.HasCountMoreThan(int,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> HasCountMoreThan<T>(
        this Task<T> origin,
        int count,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasCountMoreThan(
            count,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.HasCountMoreOrExactly(int,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> HasCountMoreOrExactly<T>(
        this Task<T> origin,
        int count,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasCountMoreOrExactly(count, details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.HasCountMoreOrExactly(int,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> HasCountMoreOrExactly<T>(
        this Task<T> origin,
        int count,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).HasCountMoreOrExactly(
            count,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.Contains(object,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Contains<T>(
        this Task<T> origin,
        object? content,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Contains(content, details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.Contains(object,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Contains<T>(
        this Task<T> origin,
        object? content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Contains(content, optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.ContainsAsync(object,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ContainsAsync<T>(
        this Task<T> origin,
        object? content,
        CancellationToken canceler,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ContainsAsync(content, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.ContainsAsync(object,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ContainsAsync<T>(
        this Task<T> origin,
        object? content,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ContainsAsync(content, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.ContainsNot(object,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ContainsNot<T>(
        this Task<T> origin,
        object? content,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ContainsNot(content, details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.ContainsNot(object,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ContainsNot<T>(
        this Task<T> origin,
        object? content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ContainsNot(
            content,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.ContainsNotAsync(object,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ContainsNotAsync<T>(
        this Task<T> origin,
        object? content,
        CancellationToken canceler,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ContainsNotAsync(content, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.ContainsNotAsync(object,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ContainsNotAsync<T>(
        this Task<T> origin,
        object? content,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ContainsNotAsync(content, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.Fail(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task Fail<T>(this Task<T> origin, string? details = null)
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        (await origin.ConfigureAwait(false)).Fail(details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.Fail(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task Fail<T>(
        this Task<T> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        (await origin.ConfigureAwait(false)).Fail(optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.Debug(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Debug<T>(this Task<T> origin, string? details = null)
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Debug(details);
    }

    /// <inheritdoc cref="AssertEnumerableBase{T}.Debug(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Debug<T>(
        this Task<T> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertEnumerableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Debug(optionConfiguration, details);
    }
}

#pragma warning restore
