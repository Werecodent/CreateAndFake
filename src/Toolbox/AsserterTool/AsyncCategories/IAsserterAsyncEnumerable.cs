using System.Collections;
using Werecodent.CreateAndFake.AsserterTool.Categories;

namespace Werecodent.CreateAndFake.AsserterTool.AsyncCategories;

#pragma warning disable CA1716 // Matches existing usage.

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

    /// <inheritdoc cref="IAsserterEnumerable.HasCountLessThan(int,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task HasCountLessThanAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountLessThan(int,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task HasCountLessThanAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountLessOrExactly(int,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task HasCountLessOrExactlyAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountLessOrExactly(int,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task HasCountLessOrExactlyAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountMoreThan(int,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task HasCountMoreThanAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountMoreThan(int,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task HasCountMoreThanAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountMoreOrExactly(int,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task HasCountMoreOrExactlyAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountMoreOrExactly(int,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task HasCountMoreOrExactlyAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.Contains(object,IEnumerable,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ContainsAsync<T>(
        T? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.Contains(object,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ContainsAsync<T>(
        T? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.ContainsNot(object,IEnumerable,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ContainsNotAsync<T>(
        T? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.ContainsNot(object,IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ContainsNotAsync<T>(
        T? content,
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

    /// <inheritdoc cref="DebugAsync{T}(IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    Task DebugAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.Debug(IEnumerable,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task DebugAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ThrowsAsync{T,T}(IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    Task<TException> ThrowsAsync<TException, TContent>(
        IAsyncEnumerable<TContent>? collection,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception;

    /// <inheritdoc cref="IAsserterValueTask.ThrowsAsync{T,T}(ValueTask{T}?,CancellationToken,string)"/>
    Task<TException> ThrowsAsync<TException, TContent>(
        IAsyncEnumerable<TContent>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception;

    /// <inheritdoc cref="ThrowsAsync{T,T}(IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    Task<Exception> ThrowsExceptionAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="ThrowsAsync{T,T}(IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    Task<Exception> ThrowsExceptionAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}

#pragma warning restore
