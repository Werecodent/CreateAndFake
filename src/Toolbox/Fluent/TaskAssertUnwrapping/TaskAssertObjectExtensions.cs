using CreateAndFake.Design;
using CreateAndFake.FakerTool;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAssertObjectExtensions
{
    /// <inheritdoc cref="AssertObjectBase{T}.Is(object,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Is<T>(
        this Task<T> origin,
        object? expected,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Is(expected, details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.Is(object,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Is<T>(
        this Task<T> origin,
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Is(expected, optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.IsNull(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsNull<T>(
        this Task<T> origin,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).IsNull(details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.IsNull(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsNull<T>(
        this Task<T> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).IsNull(optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.IsNot(object,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsNot<T>(
        this Task<T> origin,
        object? expected,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).IsNot(expected, details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.IsNot(object,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsNot<T>(
        this Task<T> origin,
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).IsNot(expected, optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.IsNotNull(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsNotNull<T>(
        this Task<T> origin,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).IsNotNull(details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.IsNotNull(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> IsNotNull<T>(
        this Task<T> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).IsNotNull(optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.ReferenceEqual(object,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ReferenceEqual<T>(
        this Task<T> origin,
        object? expected,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ReferenceEqual(expected, details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.ReferenceEqual(object,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ReferenceEqual<T>(
        this Task<T> origin,
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ReferenceEqual(
            expected,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertObjectBase{T}.ReferenceNotEqual(object,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ReferenceNotEqual<T>(
        this Task<T> origin,
        object? expected,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ReferenceNotEqual(expected, details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.ReferenceNotEqual(object,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ReferenceNotEqual<T>(
        this Task<T> origin,
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ReferenceNotEqual(
            expected,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertObjectBase{T}.ValuesEqual(object,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ValuesEqual<T>(
        this Task<T> origin,
        object? expected,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ValuesEqual(expected, details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.ValuesEqual(object,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ValuesEqual<T>(
        this Task<T> origin,
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ValuesEqual(
            expected,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertObjectBase{T}.ValuesNotEqual(object,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ValuesNotEqual<T>(
        this Task<T> origin,
        object? expected,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ValuesNotEqual(expected, details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.ValuesNotEqual(object,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> ValuesNotEqual<T>(
        this Task<T> origin,
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).ValuesNotEqual(
            expected,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertObjectBase{T}.UniqueFrom(object,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> UniqueFrom<T>(
        this Task<T> origin,
        object? expected,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).UniqueFrom(expected, details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.UniqueFrom(object,AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> UniqueFrom<T>(
        this Task<T> origin,
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).UniqueFrom(
            expected,
            optionConfiguration,
            details
        );
    }

    /// <inheritdoc cref="AssertObjectBase{T}.Fail(string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task Fail<T>(this Task<T> origin, string? details = null)
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        (await origin.ConfigureAwait(false)).Fail(details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.Fail(AsserterMod,string)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task Fail<T>(
        this Task<T> origin,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        (await origin.ConfigureAwait(false)).Fail(optionConfiguration, details);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.Pass()"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task Pass<T>(this Task<T> origin)
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        (await origin.ConfigureAwait(false)).Pass();
    }

    /// <inheritdoc cref="AssertObjectBase{T}.Pass(AsserterMod)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task Pass<T>(this Task<T> origin, AsserterMod? optionConfiguration)
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        (await origin.ConfigureAwait(false)).Pass(optionConfiguration);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.Called(AsserterMod)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Called<T>(
        this Task<T> origin,
        AsserterMod? optionConfiguration = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Called(optionConfiguration);
    }

    /// <inheritdoc cref="AssertObjectBase{T}.Called(Times?,AsserterMod)"/>
    /// <param name="origin">Assert provider in asynchronous context.</param>
    public static async Task<AssertChainer<T>> Called<T>(
        this Task<T> origin,
        Times total,
        AsserterMod? optionConfiguration = null
    )
        where T : AssertObjectBase<T>
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Called(total, optionConfiguration);
    }
}
