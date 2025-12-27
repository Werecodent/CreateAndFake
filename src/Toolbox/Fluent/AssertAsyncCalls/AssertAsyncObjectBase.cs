using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Fluent.AssertAsyncCalls;

/// <summary>Handles assertion calls for delegates.</summary>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertAsyncObjectBase<T>(IAsserter asserter, object? actual)
    : AssertObjectBase<T>(asserter, actual)
    where T : AssertAsyncObjectBase<T>
{
    /// <inheritdoc cref="IAsyncObjectAsserter.IsAsync(object,object,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public async Task<AssertChainer<T>> IsAsync(object? expected, string? details = null)
    {
        await Asserter.IsAsync(expected, Actual, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsyncObjectAsserter.IsAsync(object,object,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public async Task<AssertChainer<T>> IsAsync(
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .IsAsync(expected, Actual, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsyncObjectAsserter.IsNotAsync(object,object,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public async Task<AssertChainer<T>> IsNotAsync(object? expected, string? details = null)
    {
        await Asserter.IsNotAsync(expected, Actual, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsyncObjectAsserter.IsNotAsync(object,object,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public async Task<AssertChainer<T>> IsNotAsync(
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .IsNotAsync(expected, Actual, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsyncObjectAsserter.ValuesEqualAsync(object,object,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> ValuesEqualAsync(
        object? expected,
        string? details = null
    )
    {
        await Asserter.ValuesEqualAsync(expected, Actual, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsyncObjectAsserter.ValuesEqualAsync(object,object,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> ValuesEqualAsync(
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .ValuesEqualAsync(expected, Actual, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsyncObjectAsserter.ValuesNotEqualAsync(object,object,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> ValuesNotEqualAsync(
        object? expected,
        string? details = null
    )
    {
        await Asserter.ValuesNotEqualAsync(expected, Actual, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsyncObjectAsserter.ValuesNotEqualAsync(object,object,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> ValuesNotEqualAsync(
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .ValuesNotEqualAsync(expected, Actual, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsyncObjectAsserter.AreUniqueAsync(object,object,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> UniqueFromAsync(
        object? expected,
        string? details = null
    )
    {
        await Asserter.AreUniqueAsync(expected, Actual, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsyncObjectAsserter.AreUniqueAsync(object,object,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> UniqueFromAsync(
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .AreUniqueAsync(expected, Actual, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }
}
