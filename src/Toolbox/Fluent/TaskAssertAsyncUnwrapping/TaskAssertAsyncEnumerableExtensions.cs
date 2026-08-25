using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertAsyncEnumerableExtensions
{
    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.IsEmptyAsync(CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> IsEmptyAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .IsEmptyAsync(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.IsEmptyAsync(CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> IsEmptyAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .IsEmptyAsync(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.IsNotEmptyAsync(CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> IsNotEmptyAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .IsNotEmptyAsync(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.IsNotEmptyAsync(CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> IsNotEmptyAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .IsNotEmptyAsync(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.HasCountAsync(int,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> HasCountAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        int count,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasCountAsync(count, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.HasCountAsync(int,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> HasCountAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        int count,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasCountAsync(count, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.HasCountLessThanAsync(int,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> HasCountLessThanAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        int count,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasCountLessThanAsync(count, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.HasCountLessThanAsync(int,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> HasCountLessThanAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        int count,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasCountLessThanAsync(count, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.HasCountLessOrExactlyAsync(int,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> HasCountLessOrExactlyAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        int count,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasCountLessOrExactlyAsync(count, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.HasCountLessOrExactlyAsync(int,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> HasCountLessOrExactlyAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        int count,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasCountLessOrExactlyAsync(count, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.HasCountMoreThanAsync(int,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> HasCountMoreThanAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        int count,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasCountMoreThanAsync(count, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.HasCountMoreThanAsync(int,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> HasCountMoreThanAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        int count,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasCountMoreThanAsync(count, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.HasCountMoreOrExactlyAsync(int,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> HasCountMoreOrExactlyAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        int count,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasCountMoreOrExactlyAsync(count, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.HasCountMoreOrExactlyAsync(int,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> HasCountMoreOrExactlyAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        int count,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasCountMoreOrExactlyAsync(count, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.ContainsAsync(T,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> ContainsAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        T? content,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ContainsAsync(content, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.ContainsAsync(T,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> ContainsAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        T? content,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ContainsAsync(content, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.ContainsNotAsync(T,CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> ContainsNotAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        T? content,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ContainsNotAsync(content, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.ContainsNotAsync(T,CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> ContainsNotAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        T? content,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .ContainsNotAsync(content, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.FailAsync(CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> FailAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .FailAsync(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.FailAsync(CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> FailAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .FailAsync(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.DebugAsync(CancellationToken,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> DebugAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .DebugAsync(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertAsyncEnumerableBase{T,T}.DebugAsync(CancellationToken,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertAsyncEnumerable<T>>> DebugAsync<T>(
        this Task<AssertAsyncEnumerable<T>> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .DebugAsync(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }
}
