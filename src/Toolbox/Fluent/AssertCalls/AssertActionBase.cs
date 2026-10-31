using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.AsserterTool.Categories;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent.AssertCalls;

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
    public virtual ExceptionChainer<TException> Throws<TException>(string? details = null)
        where TException : Exception
    {
        return new ExceptionChainer<TException>(
            Asserter.Throws<TException>(Behavior, details),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterAction.Throws{T}(Action,AsserterMod,string)"/>
    public virtual ExceptionChainer<TException> Throws<TException>(
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        return new ExceptionChainer<TException>(
            Asserter.Throws<TException>(Behavior, optionConfiguration, details),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterAction.ThrowsException(Action,string)"/>
    public virtual ExceptionChainer<Exception> ThrowsException(string? details = null)
    {
        return new ExceptionChainer<Exception>(
            Asserter.ThrowsException(Behavior, details),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterAction.ThrowsException(Action,AsserterMod,string)"/>
    public virtual ExceptionChainer<Exception> ThrowsException(
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return new ExceptionChainer<Exception>(
            Asserter.ThrowsException(Behavior, optionConfiguration, details),
            Asserter
        );
    }

    /// <returns><inheritdoc cref="AssertObjectBase{T}.ToChainer" path="/returns"/></returns>
    /// <inheritdoc cref="IAsserterAction.ThrowsNo{T}(Action,string)"/>
    public virtual AlsoChainer ThrowsNo<TException>(string? details = null)
        where TException : Exception
    {
        Asserter.ThrowsNo<TException>(Behavior, details);
        return new AlsoChainer(Asserter);
    }

    /// <returns><inheritdoc cref="AssertObjectBase{T}.ToChainer" path="/returns"/></returns>
    /// <inheritdoc cref="IAsserterAction.ThrowsNo{T}(Action,AsserterMod,string)"/>
    public virtual AlsoChainer ThrowsNo<TException>(
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        Asserter.ThrowsNo<TException>(Behavior, optionConfiguration, details);
        return new AlsoChainer(Asserter);
    }

    /// <returns><inheritdoc cref="AssertObjectBase{T}.ToChainer" path="/returns"/></returns>
    /// <inheritdoc cref="IAsserterAction.ThrowsNoException(Action,string)"/>
    public virtual AlsoChainer ThrowsNoException(string? details = null)
    {
        Asserter.ThrowsNoException(Behavior, details);
        return new AlsoChainer(Asserter);
    }

    /// <returns><inheritdoc cref="AssertObjectBase{T}.ToChainer" path="/returns"/></returns>
    /// <inheritdoc cref="IAsserterAction.ThrowsNoException(Action,AsserterMod,string)"/>
    public virtual AlsoChainer ThrowsNoException(
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        Asserter.ThrowsNoException(Behavior, optionConfiguration, details);
        return new AlsoChainer(Asserter);
    }
}
