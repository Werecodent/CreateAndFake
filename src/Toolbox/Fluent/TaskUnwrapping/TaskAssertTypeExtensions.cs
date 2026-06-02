using CreateAndFake.Design;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertTypeExtensions
{
    /// <inheritdoc cref="AssertTypeBase{T}.Inherits{T}(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertType>> Inherits<TChild>(
        this Task<AssertType> origin,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Inherits<TChild>(details);
    }

    /// <inheritdoc cref="AssertTypeBase{T}.Inherits{T}(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertType>> Inherits<TChild>(
        this Task<AssertType> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Inherits<TChild>(optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertTypeBase{T}.Inherits(Type,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Inherits<T>(
        this Task<T> origin,
        Type? child,
        string? details = null
    )
        where T : AssertTypeBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Inherits(child, details);
    }

    /// <inheritdoc cref="AssertTypeBase{T}.Inherits(Type,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Inherits<T>(
        this Task<T> origin,
        Type? child,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertTypeBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Inherits(child, optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertTypeBase{T}.InheritedBy{T}(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertType>> InheritedBy<TParent>(
        this Task<AssertType> origin,
        string? details = null
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).InheritedBy<TParent>(details);
    }

    /// <inheritdoc cref="AssertTypeBase{T}.InheritedBy{T}(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<AssertType>> InheritedBy<TParent>(
        this Task<AssertType> origin,
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

    /// <inheritdoc cref="AssertTypeBase{T}.InheritedBy(Type,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> InheritedBy<T>(
        this Task<T> origin,
        Type? parent,
        string? details = null
    )
        where T : AssertTypeBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).InheritedBy(parent, details);
    }

    /// <inheritdoc cref="AssertTypeBase{T}.InheritedBy(Type,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> InheritedBy<T>(
        this Task<T> origin,
        Type? parent,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertTypeBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).InheritedBy(
            parent,
            optionConfiguration,
            details
        );
    }
}
