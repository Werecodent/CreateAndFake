using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Categories;

namespace CreateAndFake.Fluent.AssertCalls;

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="behavior">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertBehaviorBase<T>(IAsserter asserter, Delegate? behavior)
    : AssertObjectBase<T>(asserter, behavior)
    where T : AssertBehaviorBase<T>
{
    /// <summary>Delegate to run assertion checks with.</summary>
    protected Delegate? Behavior { get; } = behavior;

    /// <inheritdoc cref="IDelegateAsserter.Throws{T}(Delegate,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual TException Throws<TException>(string? details = null)
        where TException : Exception
    {
        return Asserter.Throws<TException>(Behavior, details);
    }

    /// <inheritdoc cref="IDelegateAsserter.Throws{T}(Delegate,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual TException Throws<TException>(
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        return Asserter.Throws<TException>(Behavior, optionConfiguration, details);
    }

    /// <inheritdoc cref="IDelegateAsserter.ThrowsNo{T}(Delegate,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual void ThrowsNo<TException>(string? details = null)
        where TException : Exception
    {
        Asserter.ThrowsNo<TException>(Behavior, details);
    }

    /// <inheritdoc cref="IDelegateAsserter.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual void ThrowsNo<TException>(
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        Asserter.ThrowsNo<TException>(Behavior, optionConfiguration, details);
    }
}
