using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.AsserterTool.Categories;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent.AssertCalls;

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="action">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertActionBase<T>(IAsserter asserter, Action? action)
    : AssertObjectBase<T>(asserter, action)
    where T : AssertActionBase<T>
{
    /// <summary>Delegate to run assertion checks with.</summary>
    protected Action? Action { get; } = action;

    /// <inheritdoc cref="IAsserterAction.Throws{T}(Action,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual ExceptionChainer<TException> Throws<TException>(string? details = null)
        where TException : Exception
    {
        return new ExceptionChainer<TException>(
            Asserter.Throws<TException>(Action, details),
            Asserter
        );
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
            Asserter.Throws<TException>(Action, optionConfiguration, details),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterAction.ThrowsNo{T}(Action,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AlsoChainer ThrowsNo<TException>(string? details = null)
        where TException : Exception
    {
        Asserter.ThrowsNo<TException>(Action, details);
        return new AlsoChainer(Asserter);
    }

    /// <inheritdoc cref="IAsserterAction.ThrowsNo{T}(Action,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AlsoChainer ThrowsNo<TException>(
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        Asserter.ThrowsNo<TException>(Action, optionConfiguration, details);
        return new AlsoChainer(Asserter);
    }
}
