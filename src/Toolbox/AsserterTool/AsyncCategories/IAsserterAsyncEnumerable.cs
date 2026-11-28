using System.Collections;
using Werecodent.CreateAndFake.AsserterTool.Categories;

namespace Werecodent.CreateAndFake.AsserterTool.AsyncCategories;

/// <summary>Handles common collection test scenarios.</summary>
public interface IAsserterAsyncEnumerable
{
    /// <inheritdoc cref="IAsserterEnumerable.IsEmpty(IEnumerable,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task IsEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.IsEmpty(IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task IsEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.IsNotEmpty(IEnumerable,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task IsNotEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.IsNotEmpty(IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task IsNotEmptyAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCount(int,IEnumerable,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task HasCountAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCount(int,IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task HasCountAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountLessThan(int,IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task HasCountLessThanAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountLessThan(int,IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task HasCountLessThanAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountLessOrExactly(int,IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task HasCountLessOrExactlyAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountLessOrExactly(int,IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task HasCountLessOrExactlyAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountMoreThan(int,IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task HasCountMoreThanAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountMoreThan(int,IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task HasCountMoreThanAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountMoreOrExactly(int,IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task HasCountMoreOrExactlyAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.HasCountMoreOrExactly(int,IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task HasCountMoreOrExactlyAsync<T>(
        int count,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.Contains(object,IEnumerable,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task ContainsAsync<T>(
        T? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.Contains(object,IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task ContainsAsync<T>(
        T? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.ContainsNot(object,IEnumerable,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task ContainsNotAsync<T>(
        T? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.ContainsNot(object,IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task ContainsNotAsync<T>(
        T? content,
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.Fail(IEnumerable,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task FailAsync<T>(
        IAsyncEnumerable<T>? collection,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterEnumerable.Fail(IEnumerable,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
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
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
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
