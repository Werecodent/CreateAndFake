using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertGenericTaskExtensions
{
    /// <inheritdoc cref="AssertGenericTaskBase{T,T}.HasResultAsync(CancellationToken,string)"/>
    /// <returns><inheritdoc cref="ResultChainer{T}" path="/summary"/></returns>
    public static async Task<ResultChainer<T>> HasResultAsync<T>(
        this Task<AssertGenericTask<T>> origin,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasResultAsync(canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertGenericTaskBase{T,T}.HasResultAsync(CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="ResultChainer{T}" path="/summary"/></returns>
    public static async Task<ResultChainer<T>> HasResultAsync<T>(
        this Task<AssertGenericTask<T>> origin,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasResultAsync(canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertGenericTaskBase{T,T}.HasResultAsync(CancellationToken,string)"/>
    /// <returns><inheritdoc cref="ResultChainer{T}" path="/summary"/></returns>
    public static async Task<ResultChainer<T>> HasResultAsync<T>(
        this Task<AssertGenericTask<T>> origin,
        T expected,
        CancellationToken canceler,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasResultAsync(expected, canceler, details)
            .ConfigureAwait(false);
    }

    /// <inheritdoc cref="AssertGenericTaskBase{T,T}.HasResultAsync(CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="ResultChainer{T}" path="/summary"/></returns>
    public static async Task<ResultChainer<T>> HasResultAsync<T>(
        this Task<AssertGenericTask<T>> origin,
        T expected,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return await (await origin.ConfigureAwait(false))
            .HasResultAsync(expected, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
    }
}
