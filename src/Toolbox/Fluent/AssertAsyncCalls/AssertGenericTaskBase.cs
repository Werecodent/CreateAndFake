using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.AsserterTool.AsyncCategories;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;

#pragma warning disable CA1068 // Cleaner calls.

/// <summary>Handles assertion calls for delegates.</summary>
/// <param name="behavior">Delegate to check.</param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertGenericTaskBase<TItem, TSelf>(IAsserter asserter, Task<TItem>? behavior)
    : AssertTaskBase<TSelf>(asserter, behavior)
    where TSelf : AssertGenericTaskBase<TItem, TSelf>
{
    /// <summary>Delegate to run assertion checks with.</summary>
    protected Task<TItem>? Behavior { get; } = behavior;

    /// <inheritdoc cref="IAsserterValueTask.HasResultAsync{T}(ValueTask{T}?,CancellationToken,string)"/>
    /// <returns><inheritdoc cref="ResultChainer{T}" path="/summary"/></returns>
    public virtual async Task<ResultChainer<TItem>> HasResultAsync(
        CancellationToken canceler,
        string? details = null
    )
    {
        return new ResultChainer<TItem>(
            await Asserter.HasResultAsync(Behavior, canceler, details).ConfigureAwait(false),
            Asserter
        );
    }

    /// <inheritdoc cref="IAsserterValueTask.HasResultAsync{T}(ValueTask{T}?,CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="ResultChainer{T}" path="/summary"/></returns>
    public virtual async Task<ResultChainer<TItem>> HasResultAsync(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        return new ResultChainer<TItem>(
            await Asserter
                .HasResultAsync(Behavior, canceler, optionConfiguration, details)
                .ConfigureAwait(false),
            Asserter
        );
    }
}
