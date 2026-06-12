using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent.AssertCalls;

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="behavior">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertFuncBase<TItem, TSelf>(IAsserter asserter, Func<TItem>? behavior)
    : AssertObjectBase<TSelf>(asserter, behavior)
    where TSelf : AssertFuncBase<TItem, TSelf>
{
    /// <summary>Delegate to run assertion checks with.</summary>
    protected Func<TItem>? Behavior { get; } = behavior;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual ExceptionChainer<TException> Throws<TException>(string? details = null)
        where TException : Exception
    {
        return new ExceptionChainer<TException>(Asserter.Throws<TException>(Behavior, details));
    }

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual ExceptionChainer<TException> Throws<TException>(
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        return new ExceptionChainer<TException>(
            Asserter.Throws<TException>(Behavior, optionConfiguration, details)
        );
    }

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual void ThrowsNo<TException>(string? details = null)
        where TException : Exception
    {
        Asserter.ThrowsNo<TException>(Behavior, details);
    }

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual void ThrowsNo<TException>(
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        Asserter.ThrowsNo<TException>(Behavior, optionConfiguration, details);
    }

    /// <inheritdoc cref="IAsserterFunc.HasResult{T}(Func{T},string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual ResultChainer<TItem> HasResult(string? details = null)
    {
        return new ResultChainer<TItem>(Asserter.HasResult(Behavior, details));
    }

    /// <inheritdoc cref="IAsserterFunc.HasResult{T}(Func{T},AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual ResultChainer<TItem> HasResult(
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return new ResultChainer<TItem>(Asserter.HasResult(Behavior, optionConfiguration, details));
    }
}
