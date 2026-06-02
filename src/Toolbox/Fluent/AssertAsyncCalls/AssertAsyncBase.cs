using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Fluent.AssertAsyncCalls;

#pragma warning disable CA1068 // Cleaner calls.

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="behavior">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertAsyncBase<T>(IAsserter asserter, Func<Task?>? behavior)
    : AssertAsyncObjectBase<T>(asserter, behavior)
    where T : AssertAsyncBase<T>
{
    /// <summary>Delegate to run assertion checks with.</summary>
    protected Func<Task?>? Behavior { get; } = behavior;

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Func{Task},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual Task<TException> ThrowsAsync<TException>(
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        return Asserter.ThrowsAsync<TException>(Behavior, canceler, details);
    }

    /// <inheritdoc cref="IAsserterTask.ThrowsAsync{T}(Func{Task},CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual Task<TException> ThrowsAsync<TException>(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        return Asserter.ThrowsAsync<TException>(Behavior, canceler, optionConfiguration, details);
    }

    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Func{Task},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual Task ThrowsNoAsync<TException>(
        CancellationToken canceler,
        string? details = null
    )
        where TException : Exception
    {
        return Asserter.ThrowsNoAsync<TException>(Behavior, canceler, details);
    }

    /// <inheritdoc cref="IAsserterTask.ThrowsNoAsync{T}(Func{Task},CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual Task ThrowsNoAsync<TException>(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        return Asserter.ThrowsNoAsync<TException>(Behavior, canceler, optionConfiguration, details);
    }
}

#pragma warning restore CA1068
