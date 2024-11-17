using System.Collections;
using System.Diagnostics.CodeAnalysis;
using CreateAndFake.Toolbox.AsserterTool.Categories;

namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <summary>Handles common collection assertion calls.</summary>
/// <param name="collection"><inheritdoc cref="Collection" path="/summary"/></param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertEnumerableBase<T>(IAsserter asserter, IEnumerable? collection)
    : AssertObjectBase<T>(asserter, collection) where T : AssertEnumerableBase<T>
{
    /// <summary>Collection to run assertion checks with.</summary>
    protected IEnumerable? Collection { get; } = collection;

    /// <inheritdoc cref="IEnumerableAsserter.IsEmpty(IEnumerable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> IsEmpty(string? details = null)
    {
        Asserter.IsEmpty(Collection, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IEnumerableAsserter.IsEmpty(IEnumerable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> IsEmpty(AsserterMod? optionConfiguration, string? details = null)
    {
        Asserter.IsEmpty(Collection, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IEnumerableAsserter.IsNotEmpty(IEnumerable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> IsNotEmpty(string? details = null)
    {
        Asserter.IsNotEmpty(Collection, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IEnumerableAsserter.IsNotEmpty(IEnumerable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> IsNotEmpty(AsserterMod? optionConfiguration, string? details = null)
    {
        Asserter.IsNotEmpty(Collection, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IEnumerableAsserter.HasCount(int,IEnumerable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> HasCount(int count, string? details = null)
    {
        Asserter.HasCount(count, Collection, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IEnumerableAsserter.HasCount(int,IEnumerable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> HasCount(int count, AsserterMod? optionConfiguration, string? details = null)
    {
        Asserter.HasCount(count, Collection, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IEnumerableAsserter.Contains(object,IEnumerable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> Contains(object? content, string? details = null)
    {
        Asserter.Contains(content, Collection, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IEnumerableAsserter.Contains(object,IEnumerable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> Contains(object? content, AsserterMod? optionConfiguration, string? details = null)
    {
        Asserter.Contains(content, Collection, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IEnumerableAsserter.ContainsNot(object,IEnumerable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> ContainsNot(object? content, string? details = null)
    {
        Asserter.ContainsNot(content, Collection, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IEnumerableAsserter.ContainsNot(object,IEnumerable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> ContainsNot(
        object? content, AsserterMod? optionConfiguration, string? details = null)
    {
        Asserter.ContainsNot(content, Collection, optionConfiguration, details);
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
