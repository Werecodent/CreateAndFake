using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertAsyncObjectExtensions
{
    /// <inheritdoc cref="AssertAsyncObjectBase{T}.IsAsync(object,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsAsync<T>(
        this Task<T> origin,
        object? expected,
        CancellationToken canceler,
        string? details = null
    )
        where T : AssertAsyncObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .IsAsync(expected, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncObjectBase{T}.IsAsync(object,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsAsync<T>(
        this Task<T> origin,
        object? expected,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertAsyncObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .IsAsync(expected, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncObjectBase{T}.IsNotAsync(object,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsNotAsync<T>(
        this Task<T> origin,
        object? expected,
        CancellationToken canceler,
        string? details = null
    )
        where T : AssertAsyncObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .IsNotAsync(expected, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncObjectBase{T}.IsNotAsync(object,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsNotAsync<T>(
        this Task<T> origin,
        object? expected,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertAsyncObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .IsNotAsync(expected, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncObjectBase{T}.ValuesEqualAsync(object,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ValuesEqualAsync<T>(
        this Task<T> origin,
        object? expected,
        CancellationToken canceler,
        string? details = null
    )
        where T : AssertAsyncObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ValuesEqualAsync(expected, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncObjectBase{T}.ValuesEqualAsync(object,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ValuesEqualAsync<T>(
        this Task<T> origin,
        object? expected,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertAsyncObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ValuesEqualAsync(expected, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncObjectBase{T}.ValuesNotEqualAsync(object,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ValuesNotEqualAsync<T>(
        this Task<T> origin,
        object? expected,
        CancellationToken canceler,
        string? details = null
    )
        where T : AssertAsyncObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ValuesNotEqualAsync(expected, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncObjectBase{T}.ValuesNotEqualAsync(object,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ValuesNotEqualAsync<T>(
        this Task<T> origin,
        object? expected,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertAsyncObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ValuesNotEqualAsync(expected, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncObjectBase{T}.UniqueFromAsync(object,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> UniqueFromAsync<T>(
        this Task<T> origin,
        object? expected,
        CancellationToken canceler,
        string? details = null
    )
        where T : AssertAsyncObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .UniqueFromAsync(expected, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncObjectBase{T}.UniqueFromAsync(object,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> UniqueFromAsync<T>(
        this Task<T> origin,
        object? expected,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertAsyncObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .UniqueFromAsync(expected, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

#pragma warning disable VSTHRD200 // Matches underlying method.

    /// <inheritdoc cref="AssertObjectBase{T}.Inherits{T}(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncObject>> Inherits<TChild>(
        this Task<AssertAsyncObject> origin,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Inherits<TChild>(details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.Inherits{T}(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncObject>> Inherits<TChild>(
        this Task<AssertAsyncObject> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Inherits<TChild>(optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.InheritedBy{T}(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncObject>> InheritedBy<TParent>(
        this Task<AssertAsyncObject> origin,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).InheritedBy<TParent>(details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.InheritedBy{T}(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncObject>> InheritedBy<TParent>(
        this Task<AssertAsyncObject> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).InheritedBy<TParent>(
            optionConfiguration,
            details
        );
    }

#pragma warning restore
}
