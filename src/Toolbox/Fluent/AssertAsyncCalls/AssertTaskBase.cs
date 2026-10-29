using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.AsserterTool.AsyncCategories;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;

#pragma warning disable CA1068 // Cleaner calls.

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="behavior">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertTaskBase<T>(IAsserter asserter, Task? behavior)
    : AssertAsyncObjectBase<T>(asserter, behavior)
    where T : AssertTaskBase<T>
{
    /// <summary>Delegate to run assertion checks with.</summary>
    protected Task? Behavior { get; } = behavior;

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task?,CancellationToken,string)"/>
    public virtual async Task<ExceptionChainer<TException>> ThrowsAsync<TException>(
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        return new ExceptionChainer<TException>(
            await Asserter
                .ThrowsAsync<TException>(Behavior, canceler, details)
                .ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task?,CancellationToken,AsserterMod,string)"/>
    public virtual async Task<ExceptionChainer<TException>> ThrowsAsync<TException>(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        return new ExceptionChainer<TException>(
            await Asserter
                .ThrowsAsync<TException>(Behavior, canceler, optionConfiguration, details)
                .ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterTask.ThrowsExceptionAsync(Task?,CancellationToken,string)"/>
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

    /// <inheritdoc cref="IAsserterTask.ThrowsExceptionAsync(Task?,CancellationToken,AsserterMod,string)"/>
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
    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task?,CancellationToken,string)"/>
    public virtual async Task<AlsoChainer> ThrowsNoAsync<TException>(
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        await Asserter.ThrowsNoAsync<TException>(Behavior, canceler, details).ConfigureAwait(false);
        return new AlsoChainer(Asserter);
    }

    /// <returns><inheritdoc cref="AssertObjectBase{T}.ToChainer" path="/returns"/></returns>
    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task?,CancellationToken,AsserterMod,string)"/>
    public virtual async Task<AlsoChainer> ThrowsNoAsync<TException>(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        await Asserter
            .ThrowsNoAsync<TException>(Behavior, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return new AlsoChainer(Asserter);
    }

    /// <returns><inheritdoc cref="AssertObjectBase{T}.ToChainer" path="/returns"/></returns>
    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync(Task?,CancellationToken,string)"/>
    public virtual async Task<AlsoChainer> ThrowsNoExceptionAsync(
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.ThrowsNoExceptionAsync(Behavior, canceler, details).ConfigureAwait(false);
        return new AlsoChainer(Asserter);
    }

    /// <returns><inheritdoc cref="AssertObjectBase{T}.ToChainer" path="/returns"/></returns>
    /// <inheritdoc cref="IAsserterTask.ThrowsNoExceptionAsync(Task?,CancellationToken,AsserterMod,string)"/>
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

#pragma warning restore
