using CreateAndFake.Design;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertComparableExtensions
{
    /// <inheritdoc cref="AssertComparableBase{T}.GreaterThan(IComparable,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> GreaterThan<T>(
        this Task<T> origin,
        IComparable target,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).GreaterThan(target, details);
    }

    /// <inheritdoc cref="AssertComparableBase{T}.GreaterThan(IComparable,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> GreaterThan<T>(
        this Task<T> origin,
        IComparable target,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).GreaterThan(
            target,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertComparableBase{T}.GreaterThanOrEqualTo(IComparable,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> GreaterThanOrEqualTo<T>(
        this Task<T> origin,
        IComparable target,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).GreaterThanOrEqualTo(target, details);
    }

    /// <inheritdoc cref="AssertComparableBase{T}.GreaterThanOrEqualTo(IComparable,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> GreaterThanOrEqualTo<T>(
        this Task<T> origin,
        IComparable target,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).GreaterThanOrEqualTo(
            target,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertComparableBase{T}.GreaterThanOrIs(IComparable,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> GreaterThanOrIs<T>(
        this Task<T> origin,
        IComparable target,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).GreaterThanOrIs(target, details);
    }

    /// <inheritdoc cref="AssertComparableBase{T}.GreaterThanOrIs(IComparable,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> GreaterThanOrIs<T>(
        this Task<T> origin,
        IComparable target,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).GreaterThanOrIs(
            target,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertComparableBase{T}.LessThan(IComparable,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> LessThan<T>(
        this Task<T> origin,
        IComparable target,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).LessThan(target, details);
    }

    /// <inheritdoc cref="AssertComparableBase{T}.LessThan(IComparable,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> LessThan<T>(
        this Task<T> origin,
        IComparable target,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).LessThan(target, optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertComparableBase{T}.LessThanOrEqualTo(IComparable,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> LessThanOrEqualTo<T>(
        this Task<T> origin,
        IComparable target,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).LessThanOrEqualTo(target, details);
    }

    /// <inheritdoc cref="AssertComparableBase{T}.LessThanOrEqualTo(IComparable,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> LessThanOrEqualTo<T>(
        this Task<T> origin,
        IComparable target,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).LessThanOrEqualTo(
            target,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertComparableBase{T}.LessThanOrIs(IComparable,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> LessThanOrIs<T>(
        this Task<T> origin,
        IComparable target,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).LessThanOrIs(target, details);
    }

    /// <inheritdoc cref="AssertComparableBase{T}.LessThanOrIs(IComparable,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> LessThanOrIs<T>(
        this Task<T> origin,
        IComparable target,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).LessThanOrIs(
            target,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertComparableBase{T}.InRange(IComparable,IComparable,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> InRange<T>(
        this Task<T> origin,
        IComparable min,
        IComparable max,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).InRange(min, max, details);
    }

    /// <inheritdoc cref="AssertComparableBase{T}.InRange(IComparable,IComparable,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> InRange<T>(
        this Task<T> origin,
        IComparable min,
        IComparable max,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertComparableBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).InRange(min, max, optionConfiguration, details);
    }
}
