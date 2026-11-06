namespace Werecodent.CreateAndFake.AsserterTool.AsyncCategories;

/// <summary>Handles common async test scenarios.</summary>
public interface IAsserterValueTask
{
    /// <inheritdoc cref="HasResultAsync{T}(ValueTask{T}?,CancellationToken,AsserterMod,string)"/>
    Task<T> HasResultAsync<T>(
        ValueTask<T>? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.HasResultAsync{T}(Task{T},CancellationToken,AsserterMod,string)"/>
    Task<T> HasResultAsync<T>(
        ValueTask<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="HasResultAsync{T}(T,ValueTask{T}?,CancellationToken,AsserterMod,string)"/>
    Task<T> HasResultAsync<T>(
        T content,
        ValueTask<T>? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.HasResultAsync{T}(T,Task{T},CancellationToken,AsserterMod,string)"/>
    Task<T> HasResultAsync<T>(
        T content,
        ValueTask<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ThrowsAsync{T}(ValueTask?,CancellationToken,AsserterMod,string)"/>
    Task<T> ThrowsAsync<T>(ValueTask? behavior, CancellationToken canceler, string? details = null)
        where T : Exception;

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task<T> ThrowsAsync<T>(
        ValueTask? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="ThrowsAsync{TException,TContent}(ValueTask{TContent}?,CancellationToken,AsserterMod,string)"/>
    Task<TException> ThrowsAsync<TException, TContent>(
        ValueTask<TContent>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception;

    /// <summary>
    ///     Verifies the <c>behavior</c> throws a <typeparamref name="TException"/> <see cref="Exception"/>.
    /// </summary>
    /// <typeparam name="TException"><see cref="Exception"/> <see cref="Type"/> expected to be thrown.</typeparam>
    /// <typeparam name="TContent">The <c>behavior</c>'s content <see cref="Type"/>.</typeparam>
    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task<TException> ThrowsAsync<TException, TContent>(
        ValueTask<TContent>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception;

    /// <inheritdoc cref="ThrowsExceptionAsync(ValueTask?,CancellationToken,AsserterMod,string)"/>
    Task<Exception> ThrowsExceptionAsync(
        ValueTask? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="IAsserterTask.ThrowsExceptionAsync(Task,CancellationToken,AsserterMod,string)"/>
    Task<Exception> ThrowsExceptionAsync(
        ValueTask? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ThrowsExceptionAsync(ValueTask?,CancellationToken,AsserterMod,string)"/>
    Task<Exception> ThrowsExceptionAsync<T>(
        ValueTask<T>? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <typeparam name="T">The <c>behavior</c>'s content <see cref="Type"/>.</typeparam>
    /// <inheritdoc cref="IAsserterTask.ThrowsExceptionAsync(Task,CancellationToken,AsserterMod,string)"/>
    Task<Exception> ThrowsExceptionAsync<T>(
        ValueTask<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ThrowsNoAsync{T}(ValueTask?,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoAsync<T>(ValueTask? behavior, CancellationToken canceler, string? details = null)
        where T : Exception;

    /// <typeparam name="T">The <c>behavior</c>'s content <see cref="Type"/>.</typeparam>
    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoAsync<T>(
        ValueTask? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where T : Exception;

    /// <inheritdoc cref="ThrowsNoAsync{T,TContent}(ValueTask{TContent}?,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoAsync<TException, TContent>(
        ValueTask<TContent>? behavior,
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception;

    ///<summary>
    ///     Verifies the <c>behavior</c> does not throw a <typeparamref name="TException"/> <see cref="Exception"/>.
    /// </summary>
    /// <typeparam name="TException"><see cref="Exception"/> <see cref="Type"/> expected to not be thrown.</typeparam>
    /// <typeparam name="TContent">The <c>behavior</c>'s content <see cref="Type"/>.</typeparam>
    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoAsync<TException, TContent>(
        ValueTask<TContent>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception;

    /// <inheritdoc cref="ThrowsNoExceptionAsync(ValueTask?,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoExceptionAsync(
        ValueTask? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <inheritdoc cref="ThrowsNoExceptionAsync{T}(ValueTask{T}?,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoExceptionAsync(
        ValueTask? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );

    /// <inheritdoc cref="ThrowsNoExceptionAsync{T}(ValueTask{T}?,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoExceptionAsync<T>(
        ValueTask<T>? behavior,
        CancellationToken canceler,
        string? details = null
    );

    /// <typeparam name="T">The <c>behavior</c>'s content <see cref="Type"/>.</typeparam>
    /// <inheritdoc cref="IAsserterTask.ThrowsNoExceptionAsync(Task,CancellationToken,AsserterMod,string)"/>
    Task ThrowsNoExceptionAsync<T>(
        ValueTask<T>? behavior,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    );
}
