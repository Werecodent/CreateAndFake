using System.Collections;

namespace CreateAndFake.AsserterTool.Categories;

/// <summary>Handles common collection test scenarios.</summary>
public interface IAsyncEnumerableAsserter
{
#pragma warning disable CA1716 // Identifiers should not match keywords: Matches existing usage.

    /// <inheritdoc cref="IObjectAsserter.Is(object?,object?,string?)"/>
    Task Is<T>(IAsyncEnumerable<T>? expected, IAsyncEnumerable<T>? actual, string? details = null);

    /// <inheritdoc cref="IObjectAsserter.Is(object?,object?,AsserterMod?,string?)"/>
    Task Is<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.IsNot(object?,object?,string?)"/>
    Task IsNot<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.IsNot(object?,object?,AsserterMod?,string?)"/>
    Task IsNot<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

#pragma warning restore CA1716 // Identifiers should not match keywords.

    /// <inheritdoc cref="IObjectAsserter.ValuesEqual(object?,object?,string?)"/>
    Task ValuesEqual<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.ValuesEqual(object?,object?,AsserterMod?,string?)"/>
    Task ValuesEqual<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.ValuesNotEqual(object?,object?,string?)"/>
    Task ValuesNotEqual<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.ValuesNotEqual(object?,object?,AsserterMod?,string?)"/>
    Task ValuesNotEqual<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.AreUnique(object?,object?,string?)"/>
    Task AreUnique<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        string? details = null
    );

    /// <inheritdoc cref="IObjectAsserter.AreUnique(object?,object?,AsserterMod?,string?)"/>
    Task AreUnique<T>(
        IAsyncEnumerable<T>? expected,
        IAsyncEnumerable<T>? actual,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IEnumerableAsserter.IsEmpty(IEnumerable?,string?)"/>
    Task IsEmpty<T>(IAsyncEnumerable<T>? collection, string? details = null);

    /// <inheritdoc cref="IEnumerableAsserter.IsEmpty(IEnumerable?,AsserterMod?,string?)"/>
    Task IsEmpty<T>(
        IAsyncEnumerable<T>? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IEnumerableAsserter.IsNotEmpty(IEnumerable?,string?)"/>
    Task IsNotEmpty<T>(IAsyncEnumerable<T>? collection, string? details = null);

    /// <inheritdoc cref="IEnumerableAsserter.IsNotEmpty(IEnumerable?,AsserterMod?,string?)"/>
    Task IsNotEmpty<T>(
        IAsyncEnumerable<T>? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IEnumerableAsserter.HasCount(int,IEnumerable?,string?)"/>
    Task HasCount<T>(int count, IAsyncEnumerable<T>? collection, string? details = null);

    /// <inheritdoc cref="IEnumerableAsserter.HasCount(int, IEnumerable?,AsserterMod?,string?)"/>
    Task HasCount<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IEnumerableAsserter.Contains(object?,IEnumerable?,string?)"/>
    Task Contains<T>(object? content, IAsyncEnumerable<T>? collection, string? details);

    /// <inheritdoc cref="IEnumerableAsserter.Contains(object?,IEnumerable?,AsserterMod?,string?)"/>
    Task Contains<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        AsserterMod? optionConfiguration,
        string? details
    );

    /// <inheritdoc cref="IEnumerableAsserter.ContainsNot(object?,IEnumerable?,string?)"/>
    Task ContainsNot<T>(object? content, IAsyncEnumerable<T>? collection, string? details = null);

    /// <inheritdoc cref="IEnumerableAsserter.ContainsNot(object?,IEnumerable?,AsserterMod?,string?)"/>
    Task ContainsNot<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IEnumerableAsserter.Fail(IEnumerable?,string?)"/>
    Task Fail<T>(IAsyncEnumerable<T>? collection, string? details = null);

    /// <inheritdoc cref="IEnumerableAsserter.Fail(IEnumerable?,AsserterMod?,string?)"/>
    Task Fail<T>(
        IAsyncEnumerable<T>? collection,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}
