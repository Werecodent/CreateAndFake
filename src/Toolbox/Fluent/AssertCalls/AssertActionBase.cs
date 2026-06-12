using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent.AssertCalls;

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="behavior">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertActionBase<T>(IAsserter asserter, Action? behavior)
    : AssertObjectBase<T>(asserter, behavior)
    where T : AssertActionBase<T>
{
    /// <summary>Delegate to run assertion checks with.</summary>
    protected Action? Behavior { get; } = behavior;

    /// <inheritdoc cref="IAsserterAction.Throws{T}(Action,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual ExceptionChainer<TException> Throws<TException>(string? details = null)
        where TException : Exception
    {
        return new ExceptionChainer<TException>(Asserter.Throws<TException>(Behavior, details));
    }

    /// <inheritdoc cref="IAsserterAction.Throws{T}(Action,AsserterMod,string)"/>
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

    /// <inheritdoc cref="IAsserterAction.ThrowsNo{T}(Action,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual void ThrowsNo<TException>(string? details = null)
        where TException : Exception
    {
        Asserter.ThrowsNo<TException>(Behavior, details);
    }

    /// <inheritdoc cref="IAsserterAction.ThrowsNo{T}(Action,AsserterMod,string)"/>
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
