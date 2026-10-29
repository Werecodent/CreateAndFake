using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.AsserterTool.AsyncCategories;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;

#pragma warning disable CA1068 // Cleaner calls.

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="behavior">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertGenericValueTaskBase<TItem, TSelf>(
    IAsserter asserter,
    ValueTask<TItem>? behavior
) : AssertAsyncObjectBase<TSelf>(asserter, behavior)
    where TSelf : AssertGenericValueTaskBase<TItem, TSelf>
{
    /// <summary>Delegate to run assertion checks with.</summary>
    protected ValueTask<TItem>? Behavior { get; } = behavior;

    /// <inheritdoc cref="IAsserterValueTask.HasResultAsync{T}(ValueTask{T}?,CancellationToken,string)"/>
    public virtual async Task<ResultChainer<TItem>> HasResultAsync(
        CancellationToken canceler,
        string? details = null
    )
    {
        return new ResultChainer<TItem>(
            await Asserter.HasResultAsync(Behavior, canceler, details).ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterValueTask.HasResultAsync{T}(ValueTask{T}?,CancellationToken,AsserterMod,string)"/>
    public virtual async Task<ResultChainer<TItem>> HasResultAsync(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return new ResultChainer<TItem>(
            await Asserter
                .HasResultAsync(Behavior, canceler, optionConfiguration, details)
                .ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterValueTask.HasResultAsync{T}(T,ValueTask{T}?,CancellationToken,string)"/>
    public virtual async Task<ResultChainer<TItem>> HasResultAsync(
        TItem expected,
        CancellationToken canceler,
        string? details = null
    )
    {
        return new ResultChainer<TItem>(
            await Asserter
                .HasResultAsync(expected, Behavior, canceler, details)
                .ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterValueTask.HasResultAsync{T}(T,ValueTask{T}?,CancellationToken,string)"/>
    public virtual async Task<ResultChainer<TItem>> HasResultAsync(
        TItem expected,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return new ResultChainer<TItem>(
            await Asserter
                .HasResultAsync(expected, Behavior, canceler, optionConfiguration, details)
                .ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterValueTask.ThrowsAsync{T}(ValueTask?,CancellationToken,string)"/>
    public virtual async Task<ExceptionChainer<TException>> ThrowsAsync<TException>(
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        return new ExceptionChainer<TException>(
            await Asserter
                .ThrowsAsync<TException, TItem>(Behavior, canceler, details)
                .ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterValueTask.ThrowsAsync{T}(ValueTask?,CancellationToken,AsserterMod,string)"/>
    public virtual async Task<ExceptionChainer<TException>> ThrowsAsync<TException>(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        return new ExceptionChainer<TException>(
            await Asserter
                .ThrowsAsync<TException, TItem>(Behavior, canceler, optionConfiguration, details)
                .ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterValueTask.ThrowsExceptionAsync(ValueTask?,CancellationToken,string)"/>
    public virtual async Task<ExceptionChainer<Exception>> ThrowsExceptionAsync(
        CancellationToken canceler,
        string? details = null
    )
    {
        return new ExceptionChainer<Exception>(
            await Asserter.ThrowsExceptionAsync(Behavior, canceler, details).ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterValueTask.ThrowsExceptionAsync(ValueTask?,CancellationToken,AsserterMod,string)"/>
    public virtual async Task<ExceptionChainer<Exception>> ThrowsExceptionAsync(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return new ExceptionChainer<Exception>(
            await Asserter
                .ThrowsExceptionAsync(Behavior, canceler, optionConfiguration, details)
                .ConfigureAwait(false),
            Asserter
        );
    }

    /// <returns><inheritdoc cref="AssertObjectBase{T}.ToChainer" path="/returns"/></returns>
    /// <inheritdoc cref="IAsserterValueTask.ThrowsNoAsync{T}(ValueTask?,CancellationToken,string)"/>
    public virtual async Task<AlsoChainer> ThrowsNoAsync<TException>(
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        await Asserter
            .ThrowsNoAsync<TException, TItem>(Behavior, canceler, details)
            .ConfigureAwait(false);
        return new AlsoChainer(Asserter);
    }

    /// <returns><inheritdoc cref="AssertObjectBase{T}.ToChainer" path="/returns"/></returns>
    /// <inheritdoc cref="IAsserterValueTask.ThrowsNoAsync{T}(ValueTask?,CancellationToken,AsserterMod,string)"/>
    public virtual async Task<AlsoChainer> ThrowsNoAsync<TException>(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        await Asserter
            .ThrowsNoAsync<TException, TItem>(Behavior, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return new AlsoChainer(Asserter);
    }

    /// <returns><inheritdoc cref="AssertObjectBase{T}.ToChainer" path="/returns"/></returns>
    /// <inheritdoc cref="IAsserterValueTask.ThrowsNoExceptionAsync(ValueTask?,CancellationToken,string)"/>
    public virtual async Task<AlsoChainer> ThrowsNoExceptionAsync(
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.ThrowsNoExceptionAsync(Behavior, canceler, details).ConfigureAwait(false);
        return new AlsoChainer(Asserter);
    }

    /// <returns><inheritdoc cref="AssertObjectBase{T}.ToChainer" path="/returns"/></returns>
    /// <inheritdoc cref="IAsserterValueTask.ThrowsNoExceptionAsync(ValueTask?,CancellationToken,AsserterMod,string)"/>
    public virtual async Task<AlsoChainer> ThrowsNoExceptionAsync(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .ThrowsNoExceptionAsync(Behavior, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return new AlsoChainer(Asserter);
    }
}
