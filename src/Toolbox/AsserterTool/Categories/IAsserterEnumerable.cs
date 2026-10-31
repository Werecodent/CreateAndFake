using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Werecodent.CreateAndFake.AsserterTool.Categories;

/// <summary>Handles common collection test scenarios.</summary>
public interface IAsserterEnumerable
{
    /// <inheritdoc cref="IsEmpty(IEnumerable,AsserterMod,string)"/>
    void IsEmpty(IEnumerable? collection, string? details = null);

    /// <summary>Verifies the <c>collection</c> contains no elements.</summary>
    /// <param name="collection">Instance to test.</param>
    /// <inheritdoc cref="IAsserterObject.Is(object,object,AsserterMod,string)"/>
    void IsEmpty(IEnumerable? collection, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="IsNotEmpty(IEnumerable,AsserterMod,string)"/>
    void IsNotEmpty(IEnumerable? collection, string? details = null);

    /// <summary>Verifies the <c>collection</c> contains elements.</summary>
    /// <inheritdoc cref="IsEmpty(IEnumerable,AsserterMod,string)"/>
    void IsNotEmpty(
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="HasCount(int,IEnumerable,AsserterMod,string)"/>
    void HasCount(int count, IEnumerable? collection, string? details = null);

    /// <summary>Verifies the <c>collection</c> has <paramref name="count"/> elements.</summary>
    /// <param name="count">Size constraint to verify the <c>collection</c> with.</param>
    /// <inheritdoc cref="IsEmpty(IEnumerable,AsserterMod,string)"/>
    void HasCount(
        int count,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="HasCountLessThan(int,IEnumerable,AsserterMod,string)"/>
    void HasCountLessThan(int count, IEnumerable? collection, string? details = null);

    /// <summary>Verifies <c>collection</c> has <c>&lt; count</c> elements.</summary>
    /// <inheritdoc cref="HasCount(int,IEnumerable,AsserterMod,string)"/>
    void HasCountLessThan(
        int count,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="HasCountLessOrExactly(int,IEnumerable,AsserterMod,string)"/>
    void HasCountLessOrExactly(int count, IEnumerable? collection, string? details = null);

    /// <summary>Verifies <c>collection</c> has <c>&lt;= count</c> elements.</summary>
    /// <inheritdoc cref="HasCount(int,IEnumerable,AsserterMod,string)"/>
    void HasCountLessOrExactly(
        int count,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="HasCountMoreThan(int,IEnumerable,AsserterMod,string)"/>
    void HasCountMoreThan(int count, IEnumerable? collection, string? details = null);

    /// <summary>Verifies <c>collection</c> has <c>&gt; count</c> elements.</summary>
    /// <inheritdoc cref="HasCount(int,IEnumerable,AsserterMod,string)"/>
    void HasCountMoreThan(
        int count,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="HasCountMoreOrExactly(int,IEnumerable,AsserterMod,string)"/>
    void HasCountMoreOrExactly(int count, IEnumerable? collection, string? details = null);

    /// <summary>Verifies <c>collection</c> has <c>&gt;= count</c> elements.</summary>
    /// <inheritdoc cref="HasCount(int,IEnumerable,AsserterMod,string)"/>
    void HasCountMoreOrExactly(
        int count,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="Contains(object,IEnumerable,AsserterMod,string)"/>
    void Contains(object? content, IEnumerable? collection, string? details = null);

    /// <summary>
    ///     Verifies the <c>collection</c> contains an element equal to the given <paramref name="content"/> by value.
    /// </summary>
    /// <param name="content">Expected item to be present.</param>
    /// <inheritdoc cref="IsEmpty(IEnumerable,AsserterMod,string)"/>
    void Contains(
        object? content,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ContainsAsync(object,IEnumerable,CancellationToken,AsserterMod,string)"/>
    Task ContainsAsync(
        object? content,
        IEnumerable? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="Contains(object,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task ContainsAsync(
        object? content,
        IEnumerable? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ContainsNot(object,IEnumerable,AsserterMod,string)"/>
    void ContainsNot(object? content, IEnumerable? collection, string? details = null);

    /// <summary>
    ///     Verifies the <c>collection</c> contains no element equal to given <paramref name="content"/> by value.
    /// </summary>
    /// <param name="content">Expected item to be missing.</param>
    /// <inheritdoc cref="IsEmpty(IEnumerable,AsserterMod,string)"/>
    void ContainsNot(
        object? content,
        IEnumerable? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ContainsNotAsync(object,IEnumerable,CancellationToken,AsserterMod,string)"/>
    Task ContainsNotAsync(
        object? content,
        IEnumerable? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="ContainsNot(object,IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="ContainsAsync(object,IEnumerable,CancellationToken,AsserterMod,string)"/>
    Task ContainsNotAsync(
        object? content,
        IEnumerable? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="Fail(IEnumerable,AsserterMod,string)"/>
    [DoesNotReturn]
    void Fail(IEnumerable? collection, string? details = null);

    /// <param name="collection">Collection responsible for the failure.</param>
    /// <inheritdoc cref="IAsserter.Fail(AsserterMod,string)"/>
    [DoesNotReturn]
    void Fail(IEnumerable? collection, AsserterMod? optionConfiguration, string? details = null);

    /// <inheritdoc cref="Fail(IEnumerable,AsserterMod,string)"/>
    void Debug(IEnumerable? collection, string? details = null);

    /// <param name="collection">Collection to display information for.</param>
    /// <inheritdoc cref="IAsserter.Debug(AsserterMod,string)"/>
    void Debug(IEnumerable? collection, AsserterMod? optionConfiguration, string? details = null);
}
