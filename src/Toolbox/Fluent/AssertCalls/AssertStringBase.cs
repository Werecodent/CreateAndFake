#pragma warning disable CA1307, CA1310 // Not available for all versions.

using System.Diagnostics.CodeAnalysis;
using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.Fluent.AssertCalls;

/// <summary>Handles common <see cref="string"/> assertion calls.</summary>
/// <param name="text"><inheritdoc cref="Text" path="/summary"/></param>
/// <inheritdoc cref="AssertEnumerableBase{T}"/>
public abstract class AssertStringBase<T>(IAsserter asserter, string? text)
    : AssertEnumerableBase<T>(asserter, text)
    where T : AssertStringBase<T>
{
    /// <summary>Text to run assertion checks with.</summary>
    protected string? Text { get; } = text;

    /// <inheritdoc cref="IStringAsserter.Contains(string,string,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> Contains(string content, string? details = null)
    {
        Asserter.Contains(content, Text, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IStringAsserter.Contains(string,string,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> Contains(
        string content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.Contains(content, Text, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IStringAsserter.ContainsNot(string,string,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> ContainsNot(string content, string? details = null)
    {
        Asserter.ContainsNot(content, Text, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IStringAsserter.ContainsNot(string,string,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> ContainsNot(
        string content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.ContainsNot(content, Text, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IStringAsserter.StartsWith(string,string,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> StartsWith(string content, string? details = null)
    {
        Asserter.StartsWith(content, Text, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IStringAsserter.StartsWith(string,string,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> StartsWith(
        string content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.StartsWith(content, Text, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IStringAsserter.StartsNotWith(string,string,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> StartsNotWith(string content, string? details = null)
    {
        Asserter.StartsNotWith(content, Text, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IStringAsserter.StartsNotWith(string,string,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> StartsNotWith(
        string content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.StartsNotWith(content, Text, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IStringAsserter.EndsWith(string,string,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> EndsWith(string content, string? details = null)
    {
        Asserter.EndsWith(content, Text, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IStringAsserter.EndsWith(string,string,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> EndsWith(
        string content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.EndsWith(content, Text, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IStringAsserter.EndsNotWith(string,string,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> EndsNotWith(string content, string? details = null)
    {
        Asserter.EndsNotWith(content, Text, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IStringAsserter.EndsNotWith(string,string,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> EndsNotWith(
        string content,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.EndsNotWith(content, Text, optionConfiguration, details);
        return ToChainer();
    }

    /// <inheritdoc/>
    [DoesNotReturn]
    public override void Fail(string? details = null)
    {
        Asserter.Fail(details, Text);
    }

    /// <inheritdoc/>
    [DoesNotReturn]
    public override void Fail(AsserterMod? optionConfiguration, string? details = null)
    {
        Asserter.Fail(optionConfiguration, details, Text);
    }
}

#pragma warning restore CA1307, CA1310
