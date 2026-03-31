using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.Fluent.AssertCalls;

/// <summary>Handles common comparables assertion calls.</summary>
/// <param name="value"><inheritdoc cref="Value" path="/summary"/></param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertComparableBase<T>(IAsserter asserter, IComparable? value)
    : AssertObjectBase<T>(asserter, value)
    where T : AssertComparableBase<T>
{
    /// <summary>Value to run assertion checks with.</summary>
    protected IComparable? Value { get; } = value;

    /// <inheritdoc cref="IAsserterComparable.GreaterThan(IComparable,IComparable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> GreaterThan(IComparable target, string? details = null)
    {
        Asserter.GreaterThan(target, Value, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterComparable.GreaterThan(IComparable,IComparable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> GreaterThan(
        IComparable target,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.GreaterThan(target, Value, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterComparable.GreaterThanOrEqualTo(IComparable,IComparable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> GreaterThanOrEqualTo(IComparable target, string? details = null)
    {
        Asserter.GreaterThanOrEqualTo(target, Value, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterComparable.GreaterThanOrEqualTo(IComparable,IComparable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> GreaterThanOrEqualTo(
        IComparable target,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.GreaterThanOrEqualTo(target, Value, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterComparable.GreaterThanOrIs(IComparable,IComparable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> GreaterThanOrIs(IComparable target, string? details = null)
    {
        Asserter.GreaterThanOrIs(target, Value, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterComparable.GreaterThanOrIs(IComparable,IComparable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> GreaterThanOrIs(
        IComparable target,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.GreaterThanOrIs(target, Value, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterComparable.LessThan(IComparable,IComparable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> LessThan(IComparable target, string? details = null)
    {
        Asserter.LessThan(target, Value, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterComparable.LessThan(IComparable,IComparable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> LessThan(
        IComparable target,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.LessThan(target, Value, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterComparable.LessThanOrEqualTo(IComparable,IComparable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> LessThanOrEqualTo(IComparable target, string? details = null)
    {
        Asserter.LessThanOrEqualTo(target, Value, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterComparable.LessThanOrEqualTo(IComparable,IComparable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> LessThanOrEqualTo(
        IComparable target,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.LessThanOrEqualTo(target, Value, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterComparable.LessThanOrIs(IComparable,IComparable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> LessThanOrIs(IComparable target, string? details = null)
    {
        Asserter.LessThanOrIs(target, Value, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterComparable.LessThanOrIs(IComparable,IComparable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> LessThanOrIs(
        IComparable target,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.LessThanOrIs(target, Value, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterComparable.InRange(IComparable,IComparable,IComparable,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> InRange(
        IComparable min,
        IComparable max,
        string? details = null
    )
    {
        Asserter.InRange(min, max, Value, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterComparable.InRange(IComparable,IComparable,IComparable,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> InRange(
        IComparable min,
        IComparable max,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.InRange(min, max, Value, optionConfiguration, details);
        return ToChainer();
    }
}
