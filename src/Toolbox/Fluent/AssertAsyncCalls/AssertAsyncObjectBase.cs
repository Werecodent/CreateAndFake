using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Fluent.AssertAsyncCalls;

#pragma warning disable CA1068 // Cleaner calls.

/// <summary>Handles assertion calls for delegates.</summary>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertAsyncObjectBase<T>(IAsserter asserter, object? actual)
    : AssertObjectBase<T>(asserter, actual)
    where T : AssertAsyncObjectBase<T>
{
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public async Task<AssertChainer<T>> IsAsync(
        object? expected,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.IsAsync(expected, Actual, canceler, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public async Task<AssertChainer<T>> IsAsync(
        object? expected,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .IsAsync(expected, Actual, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncObject.IsNotAsync(object,object,CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public async Task<AssertChainer<T>> IsNotAsync(
        object? expected,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.IsNotAsync(expected, Actual, canceler, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncObject.IsNotAsync(object,object,CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public async Task<AssertChainer<T>> IsNotAsync(
        object? expected,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .IsNotAsync(expected, Actual, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncObject.ValuesEqualAsync(object,object,CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> ValuesEqualAsync(
        object? expected,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.ValuesEqualAsync(expected, Actual, canceler, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncObject.ValuesEqualAsync(object,object,CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> ValuesEqualAsync(
        object? expected,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .ValuesEqualAsync(expected, Actual, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncObject.ValuesNotEqualAsync(object,object,CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> ValuesNotEqualAsync(
        object? expected,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter
            .ValuesNotEqualAsync(expected, Actual, canceler, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncObject.ValuesNotEqualAsync(object,object,CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> ValuesNotEqualAsync(
        object? expected,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .ValuesNotEqualAsync(expected, Actual, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncObject.AreUniqueAsync(object,object,CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> UniqueFromAsync(
        object? expected,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.AreUniqueAsync(expected, Actual, canceler, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncObject.AreUniqueAsync(object,object,CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> UniqueFromAsync(
        object? expected,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .AreUniqueAsync(expected, Actual, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }
}

#pragma warning restore CA1068
