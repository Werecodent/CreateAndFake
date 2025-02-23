using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.AsserterTool.Categories;

/// <summary>Handles common collection test scenarios.</summary>
public interface IEnumerableAsserter
{
    /// <inheritdoc cref="IsEmpty(IEnumerable,AsserterMod,string)"/>
    void IsEmpty(IEnumerable? collection, string? details = null);

    /// <summary>Verifies <c>collection</c> is empty.</summary>
    /// <param name="collection">Collection to run assertion checks with.</param>
    /// <inheritdoc cref="IObjectAsserter.Is(object,object,AsserterMod,string)"/>
    void IsEmpty(IEnumerable? collection, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="IsNotEmpty(IEnumerable,AsserterMod,string)"/>
    void IsNotEmpty(IEnumerable? collection, string? details = null);

    /// <summary>Verifies <c>collection</c> is not empty.</summary>
    /// <inheritdoc cref="IsEmpty(IEnumerable,AsserterMod,string)"/>
    void IsNotEmpty(IEnumerable? collection, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="HasCount(int,IEnumerable,AsserterMod,string)"/>
    void HasCount(int count, IEnumerable? collection, string? details = null);

    /// <summary>Verifies <c>collection</c> has <paramref name="count"/> elements.</summary>
    /// <param name="count">Size that the <c>collection</c> should be.</param>
    /// <inheritdoc cref="IsEmpty(IEnumerable,AsserterMod,string)"/>
    void HasCount(int count, IEnumerable? collection, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="Contains(object,IEnumerable,AsserterMod,string)"/>
    void Contains(object? content, IEnumerable? collection, string? details);

    /// <summary>Verifies <c>collection</c> contains an element equal to <paramref name="content"/> by value.</summary>
    /// <param name="content">Expected present element.</param>
    /// <inheritdoc cref="IsEmpty(IEnumerable,AsserterMod,string)"/>
    void Contains(object? content, IEnumerable? collection, AsserterMod? optionConfiguration, string? details);

    /// <inheritdoc cref="ContainsNot(object,IEnumerable,AsserterMod,string)"/>
    void ContainsNot(object? content, IEnumerable? collection, string? details = null);

    /// <summary>Verifies <c>collection</c> contains no element equal to <paramref name="content"/> by value.</summary>
    /// <param name="content">Expected missing element.</param>
    /// <inheritdoc cref="IsEmpty(IEnumerable,AsserterMod,string)"/>
    void ContainsNot(object? content, IEnumerable? collection, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="Fail(IEnumerable,AsserterMod,string)"/>
    [DoesNotReturn]
    void Fail(IEnumerable? collection, string? details = null);

    /// <param name="collection">Collection to run assertion checks with.</param>
    /// <inheritdoc cref="IAsserter.Fail(AsserterMod,string,string)"/>
    [DoesNotReturn]
    void Fail(IEnumerable? collection, AsserterMod? optionConfiguration, string? details = null);
}
