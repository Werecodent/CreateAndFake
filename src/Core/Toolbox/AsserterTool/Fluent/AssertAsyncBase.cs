using CreateAndFake.Toolbox.AsserterTool.Categories;

namespace CreateAndFake.Toolbox.AsserterTool.Fluent;

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="behavior">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertAsyncBase<T>(IAsserter asserter, Func<Task>? behavior)
    : AssertObjectBase<T>(asserter, behavior) where T : AssertAsyncBase<T>
{
    /// <summary>Delegate to run assertion checks with.</summary>
    protected Func<Task>? Behavior { get; } = behavior;

    /// <inheritdoc cref="IAsyncAsserter.ThrowsAsync{T}(Func{Task},string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual Task<TException> Throws<TException>(string? details = null) where TException : Exception
    {
        return Asserter.ThrowsAsync<TException>(Behavior, details);
    }

    /// <inheritdoc cref="IAsyncAsserter.ThrowsAsync{T}(Func{Task},AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual Task<TException> Throws<TException>(
        AsserterMod? optionConfiguration, string? details = null) where TException : Exception
    {
        return Asserter.ThrowsAsync<TException>(Behavior, optionConfiguration, details);
    }

    /// <inheritdoc cref="IDelegateAsserter.ThrowsNo{T}(Delegate,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual Task ThrowsNo<TException>(string? details = null) where TException : Exception
    {
        return Asserter.ThrowsNoAsync<TException>(Behavior, details);
    }

    /// <inheritdoc cref="IDelegateAsserter.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual Task ThrowsNo<TException>(
        AsserterMod? optionConfiguration, string? details = null) where TException : Exception
    {
        return Asserter.ThrowsNoAsync<TException>(Behavior, optionConfiguration, details);
    }
}
