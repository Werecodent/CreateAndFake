using Werecodent.CreateAndFake.AsserterTool.Categories;

namespace Werecodent.CreateAndFake.AsserterTool.AsyncCategories;

/// <summary>Handles common async test scenarios.</summary>
public interface IAsserterTask
{
    /// <inheritdoc cref="HasResultAsync{T}(Task{T},CancellationToken,AsserterMod,string)"/>
    Task<T> HasResultAsync<T>(
        Task<T>? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterDelegate.HasResult{T}(Delegate,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task<T> HasResultAsync<T>(
        Task<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="HasResultAsync{T}(T,Task{T},CancellationToken,AsserterMod,string)"/>
    Task<T> HasResultAsync<T>(
        T content,
        Task<T>? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterDelegate.HasResultAsync{T}(T,Delegate,CancellationToken,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task<T> HasResultAsync<T>(
        T content,
        Task<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ThrowsAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task<T> ThrowsAsync<T>(Task? behavior, CancellationToken canceler, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task<T> ThrowsAsync<T>(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="ThrowsExceptionAsync(Task,CancellationToken,AsserterMod,string)"/>
    Task<Exception> ThrowsExceptionAsync(
        Task? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterDelegate.ThrowsException(Delegate,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task<Exception> ThrowsExceptionAsync(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ThrowsNoAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoAsync<T>(Task? behavior, CancellationToken canceler, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoAsync<T>(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="ThrowsNoExceptionAsync(Task,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoExceptionAsync(Task? behavior, CancellationToken canceler, string? details = null);

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNoException(Delegate,AsserterMod,string)"/>
    /// <inheritdoc cref="IAsserterAsyncObject.IsAsync(object,object,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoExceptionAsync(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}
