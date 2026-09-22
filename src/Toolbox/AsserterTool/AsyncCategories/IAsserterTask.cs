using Werecodent.CreateAndFake.AsserterTool.Categories;

namespace Werecodent.CreateAndFake.AsserterTool.AsyncCategories;

/// <summary>Handles common async test scenarios.</summary>
public interface IAsserterTask
{
    /// <inheritdoc cref="IAsserterDelegate.HasResult{T}(Delegate,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> HasResultAsync<T>(
        Task<T>? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterDelegate.HasResult{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> HasResultAsync<T>(
        Task<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterDelegate.HasResultAsync{T}(T,Delegate,CancellationToken,string)"/>
    Task<T> HasResultAsync<T>(
        T content,
        Task<T>? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterDelegate.HasResultAsync{T}(T,Delegate,CancellationToken,AsserterMod,string)"/>
    Task<T> HasResultAsync<T>(
        T content,
        Task<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> ThrowsAsync<T>(Task? behavior, CancellationToken canceler, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<T> ThrowsAsync<T>(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="ThrowsAsync{T}(Task,CancellationToken,string)"/>
    Task<Exception> ThrowsExceptionAsync(
        Task? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="ThrowsAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task<Exception> ThrowsExceptionAsync(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ThrowsNoAsync<T>(Task? behavior, CancellationToken canceler, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task ThrowsNoAsync<T>(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="ThrowsNoAsync{T}(Task,CancellationToken,string)"/>
    Task ThrowsNoExceptionAsync(Task? behavior, CancellationToken canceler, string? details = null);

    /// <inheritdoc cref="ThrowsNoAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoExceptionAsync(
        Task? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}
