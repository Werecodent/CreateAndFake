using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.AsserterTool.AsyncCategories;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;

#pragma warning disable CA1068 // Cleaner calls.

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="operation">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertValueTaskBase<T>(IAsserter asserter, ValueTask? operation)
    : AssertAsyncObjectBase<T>(asserter, operation)
    where T : AssertValueTaskBase<T>
{
    /// <summary>Delegate to run assertion checks with.</summary>
    protected ValueTask? Operation { get; } = operation;

    /// <inheritdoc cref="IAsserterValueTask.ThrowsAsync{T}(ValueTask?,CancellationToken,string)"/>
    /// <returns><inheritdoc cref="ExceptionChainer{T}" path="/summary"/></returns>
    public virtual async Task<ExceptionChainer<TException>> ThrowsAsync<TException>(
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        return new ExceptionChainer<TException>(
            await Asserter
                .ThrowsAsync<TException>(Operation, canceler, details)
                .ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterValueTask.ThrowsAsync{T}(ValueTask?,CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="ExceptionChainer{T}" path="/summary"/></returns>
    public virtual async Task<ExceptionChainer<TException>> ThrowsAsync<TException>(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        return new ExceptionChainer<TException>(
            await Asserter
                .ThrowsAsync<TException>(Operation, canceler, optionConfiguration, details)
                .ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterValueTask.ThrowsExceptionAsync(ValueTask?,CancellationToken,string)"/>
    /// <returns><inheritdoc cref="ExceptionChainer{T}" path="/summary"/></returns>
    public virtual async Task<ExceptionChainer<Exception>> ThrowsExceptionAsync(
        CancellationToken canceler,
        string? details = null
    )
    {
        return new ExceptionChainer<Exception>(
            await Asserter.ThrowsExceptionAsync(Operation, canceler, details).ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterValueTask.ThrowsExceptionAsync(ValueTask?,CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="ExceptionChainer{T}" path="/summary"/></returns>
    public virtual async Task<ExceptionChainer<Exception>> ThrowsExceptionAsync(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return new ExceptionChainer<Exception>(
            await Asserter
                .ThrowsExceptionAsync(Operation, canceler, optionConfiguration, details)
                .ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterValueTask.ThrowsNoAsync{T}(ValueTask?,CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AlsoChainer> ThrowsNoAsync<TException>(
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        await Asserter
            .ThrowsNoAsync<TException>(Operation, canceler, details)
            .ConfigureAwait(false);
        return new AlsoChainer(Asserter);
    }

    /// <inheritdoc cref="IAsserterValueTask.ThrowsNoAsync{T}(ValueTask?,CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AlsoChainer> ThrowsNoAsync<TException>(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        await Asserter
            .ThrowsNoAsync<TException>(Operation, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return new AlsoChainer(Asserter);
    }

    /// <inheritdoc cref="IAsserterValueTask.ThrowsNoExceptionAsync(ValueTask?,CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AlsoChainer> ThrowsNoExceptionAsync(
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.ThrowsNoExceptionAsync(Operation, canceler, details).ConfigureAwait(false);
        return new AlsoChainer(Asserter);
    }

    /// <inheritdoc cref="IAsserterValueTask.ThrowsNoAsync(ValueTask?,CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AlsoChainer> ThrowsNoExceptionAsync(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .ThrowsNoExceptionAsync(Operation, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return new AlsoChainer(Asserter);
    }
}

#pragma warning restore
