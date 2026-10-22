using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.AsserterTool.Categories;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent.AssertCalls;

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="function">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertFuncBase<TItem, TSelf>(IAsserter asserter, Func<TItem>? function)
    : AssertDelegateBase<TSelf>(asserter, function)
    where TSelf : AssertFuncBase<TItem, TSelf>
{
    /// <summary>Delegate to run assertion checks with.</summary>
    protected Func<TItem>? Function { get; } = function;

    /// <inheritdoc cref="IAsserterFunc.HasResult{T}(Func{T},string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual ResultChainer<TItem> HasResult(string? details = null)
    {
        return new ResultChainer<TItem>(Asserter.HasResult(Function, details), Asserter);
    }

    /// <inheritdoc cref="IAsserterFunc.HasResult{T}(Func{T},AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual ResultChainer<TItem> HasResult(
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return new ResultChainer<TItem>(
            Asserter.HasResult(Function, optionConfiguration, details),
            Asserter
        );
    }
}
