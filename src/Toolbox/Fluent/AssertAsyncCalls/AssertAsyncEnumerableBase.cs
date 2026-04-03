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

    /// <inheritdoc cref="IAsserterAsyncEnumerable.IsEmptyAsync{T}(IAsyncEnumerable{T},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> IsEmptyAsync(
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.IsEmptyAsync(Collection, canceler, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.IsEmptyAsync{T}(IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
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

    /// <inheritdoc cref="IAsserterAsyncEnumerable.IsNotEmptyAsync{T}(IAsyncEnumerable{T},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> IsNotEmptyAsync(
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.IsNotEmptyAsync(Collection, canceler, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.IsNotEmptyAsync{T}(IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
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

    /// <inheritdoc cref="IAsserterAsyncEnumerable.HasCountAsync{T}(int,IAsyncEnumerable{T},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> HasCountAsync(
        int count,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.HasCountAsync(count, Collection, canceler, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.HasCountAsync{T}(int,IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> HasCountAsync(
        int count,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .HasCountAsync(count, Collection, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.HasCountLessThanAsync{T}(int,IAsyncEnumerable{T},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> HasCountLessThanAsync(
        int count,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter
            .HasCountLessThanAsync(count, Collection, canceler, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.HasCountLessThanAsync{T}(int,IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> HasCountLessThanAsync(
        int count,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .HasCountLessThanAsync(count, Collection, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.HasCountLessOrExactlyAsync{T}(int,IAsyncEnumerable{T},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> HasCountLessOrExactlyAsync(
        int count,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter
            .HasCountLessOrExactlyAsync(count, Collection, canceler, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.HasCountLessOrExactlyAsync{T}(int,IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> HasCountLessOrExactlyAsync(
        int count,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .HasCountLessOrExactlyAsync(count, Collection, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.HasCountMoreThanAsync{T}(int,IAsyncEnumerable{T},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> HasCountMoreThanAsync(
        int count,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter
            .HasCountMoreThanAsync(count, Collection, canceler, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.HasCountMoreThanAsync{T}(int,IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> HasCountMoreThanAsync(
        int count,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .HasCountMoreThanAsync(count, Collection, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.HasCountMoreOrExactlyAsync{T}(int,IAsyncEnumerable{T},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> HasCountMoreOrExactlyAsync(
        int count,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter
            .HasCountMoreOrExactlyAsync(count, Collection, canceler, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.HasCountMoreOrExactlyAsync{T}(int,IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> HasCountMoreOrExactlyAsync(
        int count,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .HasCountMoreOrExactlyAsync(count, Collection, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.ContainsAsync{T}(T,IAsyncEnumerable{T},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> ContainsAsync(
        TItem? content,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.ContainsAsync(content, Collection, canceler, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.ContainsAsync{T}(T,IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> ContainsAsync(
        TItem? content,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .ContainsAsync(content, Collection, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.ContainsNotAsync{T}(T,IAsyncEnumerable{T},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> ContainsNotAsync(
        TItem? content,
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter
            .ContainsNotAsync(content, Collection, canceler, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.ContainsNotAsync{T}(T,IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> ContainsNotAsync(
        TItem? content,
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .ContainsNotAsync(content, Collection, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.FailAsync{T}(IAsyncEnumerable{T},CancellationToken,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> FailAsync(
        CancellationToken canceler,
        string? details = null
    )
    {
        await Asserter.FailAsync(Collection, canceler, details).ConfigureAwait(false);
        return ToChainer();
    }

    /// <inheritdoc cref="IAsserterAsyncEnumerable.FailAsync{T}(IAsyncEnumerable{T},CancellationToken,AsserterMod,string)"/>
    /// <returns><inheritdoc cref="AssertChainer{T}" path="/summary"/></returns>
    public virtual async Task<AssertChainer<TSelf>> FailAsync(
        CancellationToken canceler,
        AsserterMod? optionConfiguration,
        string? details = null
    )
    {
        await Asserter
            .FailAsync(Collection, canceler, optionConfiguration, details)
            .ConfigureAwait(false);
        return ToChainer();
    }
}

#pragma warning restore CA1068
