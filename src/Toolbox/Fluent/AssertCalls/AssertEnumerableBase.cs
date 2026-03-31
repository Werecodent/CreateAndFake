using System.Collections;
using System.Diagnostics.CodeAnalysis;
using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.Fluent.AssertCalls;

#pragma warning disable CA1068 // Cleaner calls.

/// <summary>Handles common collection assertion calls.</summary>
/// <param name="collection"><inheritdoc cref="Collection" path="/summary"/></param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertEnumerableBase<T>(IAsserter asserter, IEnumerable? collection)
    : AssertObjectBase<T>(asserter, collection)
    where T : AssertEnumerableBase<T>
{
    /// <summary>Collection to run assertion checks with.</summary>
    protected IEnumerable? Collection { get; } = collection;

    /// <inheritdoc cref="IAsserterEnumerable.IsEmpty(IEnumerable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> IsEmpty(string? details = null)
    {
        Asserter.IsEmpty(Collection, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterEnumerable.IsEmpty(IEnumerable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> IsEmpty(
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.IsEmpty(Collection, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterEnumerable.IsNotEmpty(IEnumerable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> IsNotEmpty(string? details = null)
    {
        Asserter.IsNotEmpty(Collection, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterEnumerable.IsNotEmpty(IEnumerable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> IsNotEmpty(
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.IsNotEmpty(Collection, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterEnumerable.HasCount(int,IEnumerable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> HasCount(int count, string? details = null)
    {
        Asserter.HasCount(count, Collection, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterEnumerable.HasCount(int,IEnumerable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> HasCount(
        int count,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.HasCount(count, Collection, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterEnumerable.Contains(object,IEnumerable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> Contains(object? content, string? details = null)
    {
        Asserter.Contains(content, Collection, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterEnumerable.Contains(object,IEnumerable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> Contains(
        object? content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.Contains(content, Collection, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterEnumerable.ContainsAsync(object,IEnumerable,CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> ContainsAsync(
        object? content,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.ContainsAsync(content, Collection, canceler, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterEnumerable.ContainsAsync(object,IEnumerable,CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> ContainsAsync(
        object? content,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .ContainsAsync(content, Collection, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterEnumerable.ContainsNot(object,IEnumerable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> ContainsNot(object? content, string? details = null)
    {
        Asserter.ContainsNot(content, Collection, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterEnumerable.ContainsNot(object,IEnumerable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> ContainsNot(
        object? content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.ContainsNot(content, Collection, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterEnumerable.ContainsNotAsync(object,IEnumerable,CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> ContainsNotAsync(
        object? content,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter
            .ContainsNotAsync(content, Collection, canceler, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterEnumerable.ContainsNotAsync(object,IEnumerable,CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<T>> ContainsNotAsync(
        object? content,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .ContainsNotAsync(content, Collection, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc/>
    [DoesNotReturn]
    public override void Fail(string? details = null)
    {
        Asserter.Fail(Collection, details);
    }

    /// <inheritdoc/>
    [DoesNotReturn]
    public override void Fail(AsserterMod? optionConfiguration, string? details = null)
    {
        Asserter.Fail(Collection, optionConfiguration, details);
    }
}

#pragma warning restore CA1068
