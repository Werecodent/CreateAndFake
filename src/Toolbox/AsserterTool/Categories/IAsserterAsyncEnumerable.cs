using System.Collections;

namespace CreateAndFake.AsserterTool.Categories;

#pragma warning disable CA1716 // Matches existing usage.
#pragma warning disable CA1068 // Cleaner calls.

/// <summary>Handles common collection test scenarios.</summary>
public interface IAsserterAsyncEnumerable
{
    /// <inheritdoc cref="IAsserterEnumerable.IsEmpty(IEnumerable,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task IsEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.IsEmpty(IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task IsEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.IsNotEmpty(IEnumerable,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task IsNotEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.IsNotEmpty(IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task IsNotEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCount(int,IEnumerable,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task HasCountAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCount(int,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task HasCountAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.Contains(object,IEnumerable,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ContainsAsync<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details
    );

    /// <inheritdoc cref="IAsserterEnumerable.Contains(object,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ContainsAsync<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details
    );

    /// <inheritdoc cref="IAsserterEnumerable.ContainsNot(object,IEnumerable,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ContainsNotAsync<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.ContainsNot(object,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ContainsNotAsync<T>(
        object? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.Fail(IEnumerable,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task FailAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.Fail(IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task FailAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}

#pragma warning restore CA1716, CA1068
