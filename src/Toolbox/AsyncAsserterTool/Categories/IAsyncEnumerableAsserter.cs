using System.Collections;
using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.AsyncAsserterTool.Categories;

#pragma warning disable CA1716 // Matches existing usage.

/// <summary>Handles common collection test scenarios.</summary>
public interface IAsyncEnumerableAsserter
{
    /// <inheritdoc cref="IEnumerableAsserter.IsEmpty(IEnumerable,string)"/>
    Task IsEmptyAsync<T>(IAsyncEnumerable<T>? collection, string? details = null);

    /// <inheritdoc cref="IEnumerableAsserter.IsEmpty(IEnumerable,AsserterMod,string)"/>
    Task IsEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IEnumerableAsserter.IsNotEmpty(IEnumerable,string)"/>
    Task IsNotEmptyAsync<T>(IAsyncEnumerable<T>? collection, string? details = null);

    /// <inheritdoc cref="IEnumerableAsserter.IsNotEmpty(IEnumerable,AsserterMod,string)"/>
    Task IsNotEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IEnumerableAsserter.HasCount(int,IEnumerable,string)"/>
    Task HasCountAsync<T>(int count, IAsyncEnumerable<T>? collection, string? details = null);

    /// <inheritdoc cref="IEnumerableAsserter.HasCount(int,IEnumerable,AsserterMod,string)"/>
    Task HasCountAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IEnumerableAsserter.Contains(object,IEnumerable,string)"/>
    Task ContainsAsync<T>(object? content, IAsyncEnumerable<T>? collection, string? details);

    /// <inheritdoc cref="IEnumerableAsserter.Contains(object,IEnumerable,AsserterMod,string)"/>
    Task ContainsAsync<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details
    );

    /// <inheritdoc cref="IEnumerableAsserter.ContainsNot(object,IEnumerable,string)"/>
    Task ContainsNotAsync<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        string? details = null
    );

    /// <inheritdoc cref="IEnumerableAsserter.ContainsNot(object,IEnumerable,AsserterMod,string)"/>
    Task ContainsNotAsync<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IEnumerableAsserter.Fail(IEnumerable,string)"/>
    Task FailAsync<T>(IAsyncEnumerable<T>? collection, string? details = null);

    /// <inheritdoc cref="IEnumerableAsserter.Fail(IEnumerable,AsserterMod,string)"/>
    Task FailAsync<T>(
        IAsyncEnumerable<T>? collection,
        AsyncAsserterMod? optionConfiguration,
        string? details = null
    );
}

#pragma warning restore CA1716
