using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent.AssertCalls;

#pragma warning disable CA1716 // Overriding here should be a rarity.

/// <summary>Handles common <see cref="Type"/> assertion calls.</summary>
/// <param name="type"><inheritdoc cref="Type" path="/summary"/></param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertTypeBase<T>(IAsserter asserter, Type? type)
    : AssertObjectBase<T>(asserter, type)
    where T : AssertTypeBase<T>
{
    /// <summary>Type to run assertion checks with.</summary>
    protected Type? Type { get; } = type;

    /// <inheritdoc cref="IAsserterType.Inherits{T}(Type,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> Inherits<TChild>(string? details = null)
    {
        Asserter.Inherits<TChild>(Type, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterType.Inherits{T}(Type,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> Inherits<TChild>(
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.Inherits<TChild>(Type, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterType.Inherits(Type,Type,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> Inherits(Type? child, string? details = null)
    {
        Asserter.Inherits(child, Type, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterType.Inherits(Type,Type,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> Inherits(
        Type? child,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.Inherits(child, Type, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterType.InheritedBy{T}(Type,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> InheritedBy<TParent>(string? details = null)
    {
        Asserter.InheritedBy<TParent>(Type, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterType.InheritedBy{T}(Type,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> InheritedBy<TParent>(
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.InheritedBy<TParent>(Type, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterType.InheritedBy(Type,Type,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> InheritedBy(Type? parent, string? details = null)
    {
        Asserter.InheritedBy(parent, Type, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterType.InheritedBy(Type,Type,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> InheritedBy(
        Type? parent,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.InheritedBy(parent, Type, optionConfiguration, details);
        return ToChainer();
    }
}

#pragma warning restore CA1716
