namespace Werecodent.CreateAndFake.AsserterTool.AsyncCategories;

#pragma warning disable CS1591

/// <summary>Handles common async test scenarios.</summary>
public interface IAsserterValueTask
{
    /// <inheritdoc cref="IAsserterTask.HasResultAsync{T}(Task{T},CancellationToken,string)"/>
    Task<T> HasResultAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.HasResultAsync{T}(Task{T},CancellationToken,AsserterMod,string)"/>
    Task<T> HasResultAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.HasResultAsync{T}(T,Task{T},CancellationToken,string)"/>
    Task<T> HasResultAsync<T>(
        T content,
        ValueTask<T>? operation,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.HasResultAsync{T}(T,Task{T},CancellationToken,AsserterMod,string)"/>
    Task<T> HasResultAsync<T>(
        T content,
        ValueTask<T>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task,CancellationToken,string)"/>
    Task<T> ThrowsAsync<T>(ValueTask? operation, CancellationToken canceler, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task<T> ThrowsAsync<T>(
        ValueTask? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task,CancellationToken,string)"/>
    Task<TException> ThrowsAsync<TException, TContent>(
        ValueTask<TContent>? operation,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception;

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task<TException> ThrowsAsync<TException, TContent>(
        ValueTask<TContent>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception;

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task,CancellationToken,string)"/>
    Task<Exception> ThrowsExceptionAsync(
        ValueTask? operation,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task<Exception> ThrowsExceptionAsync(
        ValueTask? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task,CancellationToken,string)"/>
    Task<Exception> ThrowsExceptionAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task<Exception> ThrowsExceptionAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task,CancellationToken,string)"/>
    Task ThrowsNoAsync<T>(ValueTask? operation, CancellationToken canceler, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoAsync<T>(
        ValueTask? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task,CancellationToken,string)"/>
    Task ThrowsNoAsync<TException, TContent>(
        ValueTask<TContent>? operation,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception;

    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoAsync<TException, TContent>(
        ValueTask<TContent>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception;

    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task,CancellationToken,string)"/>
    Task ThrowsNoExceptionAsync(
        ValueTask? operation,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoExceptionAsync(
        ValueTask? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task,CancellationToken,string)"/>
    Task ThrowsNoExceptionAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoExceptionAsync<T>(
        ValueTask<T>? operation,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}

#pragma warning restore
