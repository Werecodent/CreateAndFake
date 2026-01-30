using CreateAndFake.AsserterTool;
using CreateAndFake.AsserterTool.Categories;
using CreateAndFake.Fluent.AssertCalls;

namespace CreateAndFake.Fluent.AssertAsyncCalls;

#pragma warning disable CA1068 // Cleaner calls.

/// <summary>Handles common collection assertion calls.</summary>
/// <param name="collection"><inheritdoc cref="Collection" path="/summary"/></param>
/// <inheritdoc cref="AssertObjectBase{T}"/>
public abstract class AssertAsyncEnumerableBase<TItem, TSelf>(
    IAsserter asserter,
    IAsyncEnumerable<TItem>? collection
) : AssertAsyncObjectBase<TSelf>(asserter, collection)
    where TSelf : AssertAsyncEnumerableBase<TItem, TSelf>
{
    /// <summary>Collection to run assertion checks with.</summary>
    protected IAsyncEnumerable<TItem>? Collection { get; } = collection;

    /// <inheritdoc cref="IAsyncEnumerableAsserter.IsEmptyAsync{T}(IAsyncEnumerable{T},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> IsEmptyAsync(
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.IsEmptyAsync(Collection, canceler, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsyncEnumerableAsserter.IsEmptyAsync{T}(IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> IsEmptyAsync(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .IsEmptyAsync(Collection, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsyncEnumerableAsserter.IsNotEmptyAsync{T}(IAsyncEnumerable{T},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> IsNotEmptyAsync(
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.IsNotEmptyAsync(Collection, canceler, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsyncEnumerableAsserter.IsNotEmptyAsync{T}(IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> IsNotEmptyAsync(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .IsNotEmptyAsync(Collection, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }
}

#pragma warning restore CA1068
