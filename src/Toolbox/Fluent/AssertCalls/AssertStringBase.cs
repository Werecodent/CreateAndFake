#pragma warning disable CA1307, CA1310 // Not available for all .NET versions.

using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.AsserterTool.Categories;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent.AssertCalls;

/// <summary>Handles common <see cref="string"/> assertion calls.</summary>
/// <param name="text"><inheritdoc cref="Text" path="/summary"/></param>
/// <inheritdoc cref="AssertEnumerableBase{T}"/>
public abstract class AssertStringBase<T>(IAsserter asserter, string? text)
    : AssertEnumerableBase<T>(asserter, text)
    where T : AssertStringBase<T>
{
    /// <summary>Text to run assertion checks with.</summary>
    protected string? Text { get; } = text;

    /// <inheritdoc cref="IAsserterString.Contains(string,string,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> Contains(string content, string? details = null)
    {
        Asserter.Contains(content, Text, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterString.Contains(string,string,AsserterMod,string)"/>
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

    /// <inheritdoc cref="IAsserterString.ContainsNot(string,string,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> ContainsNot(string content, string? details = null)
    {
        Asserter.ContainsNot(content, Text, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterString.ContainsNot(string,string,AsserterMod,string)"/>
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

    /// <inheritdoc cref="IAsserterString.StartsWith(string,string,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> StartsWith(string content, string? details = null)
    {
        Asserter.StartsWith(content, Text, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterString.StartsWith(string,string,AsserterMod,string)"/>
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

    /// <inheritdoc cref="IAsserterString.StartsNotWith(string,string,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> StartsNotWith(string content, string? details = null)
    {
        Asserter.StartsNotWith(content, Text, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterString.StartsNotWith(string,string,AsserterMod,string)"/>
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

    /// <inheritdoc cref="IAsserterString.EndsWith(string,string,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> EndsWith(string content, string? details = null)
    {
        Asserter.EndsWith(content, Text, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterString.EndsWith(string,string,AsserterMod,string)"/>
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

    /// <inheritdoc cref="IAsserterString.EndsNotWith(string,string,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AssertChainer<T> EndsNotWith(string content, string? details = null)
    {
        Asserter.EndsNotWith(content, Text, details);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterString.EndsNotWith(string,string,AsserterMod,string)"/>
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
}

#pragma warning restore
