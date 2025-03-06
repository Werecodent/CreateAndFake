using System.Diagnostics.CodeAnalysis;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.FakerTool;

namespace CreateAndFake.AsserterTool.Fluent;

/// <summary>Handles common <see cref="object"/> assertion calls.</summary>
/// <param name="asserter"><inheritdoc cref="Asserter" path="/summary"/></param>
/// <param name="actual"><inheritdoc cref="Actual" path="/summary"/></param>
public abstract class AssertObjectBase<T>(IAsserter asserter, object? actual)
    where T : AssertObjectBase<T>
{
    /// <summary>Handles the actual assert behavior.</summary>
    protected IAsserter Asserter { get; } =
        asserter ?? throw new ArgumentNullException(nameof(asserter));

    /// <summary>Instance to run assertion checks with.</summary>
    protected object? Actual { get; } = actual;

    /// <inheritdoc cref="IObjectAsserter.Is(object,object,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public AssertChainer<T> Is(object? expected, string? details = null)
    {
        asserter.Is(expected, Actual, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.Is(object,object,AsserterMod,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public AssertChainer<T> Is(
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        asserter.Is(expected, Actual, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.IsNot(object,object,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public AssertChainer<T> IsNot(object? expected, string? details = null)
    {
        asserter.IsNot(expected, Actual, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.IsNot(object,object,AsserterMod,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public AssertChainer<T> IsNot(
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        asserter.IsNot(expected, Actual, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.ReferenceEqual(object,object,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public virtual AssertChainer<T> ReferenceEqual(object? expected, string? details = null)
    {
        asserter.ReferenceEqual(expected, Actual, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.ReferenceEqual(object,object,AsserterMod,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public virtual AssertChainer<T> ReferenceEqual(
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        asserter.ReferenceEqual(expected, Actual, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.ReferenceNotEqual(object,object,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public virtual AssertChainer<T> ReferenceNotEqual(object? expected, string? details = null)
    {
        asserter.ReferenceNotEqual(expected, Actual, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.ReferenceNotEqual(object,object,AsserterMod,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public virtual AssertChainer<T> ReferenceNotEqual(
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        asserter.ReferenceNotEqual(expected, Actual, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.ValuesEqual(object,object,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public virtual AssertChainer<T> ValuesEqual(object? expected, string? details = null)
    {
        asserter.ValuesEqual(expected, Actual, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.ValuesEqual(object,object,AsserterMod,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public virtual AssertChainer<T> ValuesEqual(
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        asserter.ValuesEqual(expected, Actual, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.ValuesNotEqual(object,object,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public virtual AssertChainer<T> ValuesNotEqual(object? expected, string? details = null)
    {
        asserter.ValuesNotEqual(expected, Actual, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.ValuesNotEqual(object,object,AsserterMod,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public virtual AssertChainer<T> ValuesNotEqual(
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        asserter.ValuesNotEqual(expected, Actual, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.AreUnique(object,object,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public virtual AssertChainer<T> UniqueFrom(object? expected, string? details = null)
    {
        asserter.AreUnique(expected, Actual, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.AreUnique(object,object,AsserterMod,string)"/>
    /// <inheritdoc cref="ToChainer"/>
    public virtual AssertChainer<T> UniqueFrom(
        object? expected,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        asserter.AreUnique(expected, Actual, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserter.Fail(string,string)"/>
    [DoesNotReturn]
    public virtual void Fail(string? details = null)
    {
        asserter.Fail(details, Actual?.ToString());
    }

    /// <inheritdoc cref="IAsserter.Fail(AsserterMod,string,string)"/>
    [DoesNotReturn]
    public virtual void Fail(AsserterMod? optionConfiguration, string? details = null)
    {
        asserter.Fail(optionConfiguration, details, Actual?.ToString());
    }

    /// <inheritdoc cref="IAsserter.Pass()"/>
    public virtual void Pass()
    {
        asserter.Pass();
    }

    /// <inheritdoc cref="IAsserter.Pass(AsserterMod)"/>
    public virtual void Pass(AsserterMod? optionConfiguration)
    {
        asserter.Pass(optionConfiguration);
    }

    /// <inheritdoc cref="IObjectAsserter.Called(object, Times?)"/>
    public virtual AssertChainer<T> Called(Times? total = null)
    {
        asserter.Called(Actual, total);
        return ToChainer();
    }

    /// <inheritdoc cref="IObjectAsserter.Called(object,AsserterMod,Times?)"/>
    public virtual AssertChainer<T> Called(
        object fake,
        AsserterMod? optionConfiguration,
        Times? total = null
    )
    {
        asserter.Called(Actual, optionConfiguration, total);
        return ToChainer();
    }

    /// <summary>Converts <c>this</c> to a chainer for additional assertions on <c>actual</c>.</summary>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    protected internal AssertChainer<T> ToChainer()
    {
        return new AssertChainer<T>((T)this, Asserter);
    }
}
