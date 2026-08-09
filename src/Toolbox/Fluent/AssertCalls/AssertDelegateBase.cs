using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.AsserterTool.Categories;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent.AssertCalls;

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="behavior">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertDelegateBase<T>(IAsserter asserter, Delegate? behavior)
    : AssertObjectBase<T>(asserter, behavior)
    where T : AssertDelegateBase<T>
{
    /// <summary>Delegate to run assertion checks with.</summary>
    protected Delegate? Behavior { get; } = behavior;

    /// <inheritdoc cref="IAsserterDelegate.Throws{T}(Delegate,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual ExceptionChainer<TException> Throws<TException>(string? details = null)
        where TException : Exception
    {
        return new ExceptionChainer<TException>(
            Asserter.Throws<TException>(Behavior, details),
            Asserter
        );
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
            Asserter.Throws<TException>(Behavior, optionConfiguration, details),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AlsoChainer ThrowsNo<TException>(string? details = null)
        where TException : Exception
    {
        Asserter.ThrowsNo<TException>(Behavior, details);
        return new AlsoChainer(Asserter);
    }

    /// <inheritdoc cref="IAsserterDelegate.ThrowsNo{T}(Delegate,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual AlsoChainer ThrowsNo<TException>(
        AsserterMod? optionConfiguration,
        string? details = null
    )
        where TException : Exception
    {
        Asserter.ThrowsNo<TException>(Behavior, optionConfiguration, details);
        return new AlsoChainer(Asserter);
    }

    /// <inheritdoc cref="HasResult{T}(AsserterMod,string)"/>
    public virtual ResultChainer<TResult> HasResult<TResult>(string? details = null)
    {
        return new ResultChainer<TResult>(Asserter.HasResult<TResult>(Behavior, details), Asserter);
    }

    /// <inheritdoc cref="IAsserterDelegate.HasResult{T}(Delegate,AsserterMod,string)"/>
    /// <typeparam name="TResult">Expected return <see cref="Type"/> of the <see cref="Behavior"/>.</typeparam>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual ResultChainer<TResult> HasResult<TResult>(
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return new ResultChainer<TResult>(
            Asserter.HasResult<TResult>(Behavior, optionConfiguration, details),
            Asserter
        );
    }
}
