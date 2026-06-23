using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.AsyncCategories;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent.AssertAsyncCalls;

#pragma warning disable CA1068 // Cleaner calls.

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="operation">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertTaskBase<T>(IAsserter asserter, Task? operation)
    : AssertAsyncObjectBase<T>(asserter, operation)
    where T : AssertTaskBase<T>
{
    /// <summary>Delegate to run assertion checks with.</summary>
    protected Task? Operation { get; } = operation;

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task?,CancellationToken,string)"/>
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

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Task?,CancellationToken,AsserterMod,string)"/>
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

    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task?,CancellationToken,string)"/>
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

    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Task?,CancellationToken,AsserterMod,string)"/>
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
}

#pragma warning restore CA1068
