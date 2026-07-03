using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.Fluent.AssertAsyncCalls;
using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAlsoChainerExtensions
{
    /// <inheritdoc cref="AssertAsyncEnumerable{T}"/>
    /// <param name="actual"><inheritdoc cref="AssertAsyncEnumerableBase{T,T}.Collection" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertAsyncEnumerable<TItem>> Also<TSelf, TItem>(
        this Task<TSelf> origin,
        IAsyncEnumerable<TItem> actual
    )
        where TSelf : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual);
    }

    /// <inheritdoc cref="AssertObject"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertAsyncObject> Also<T>(this Task<T> origin, Func<object?> actual)
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual.Invoke());
    }

    /// <inheritdoc cref="AssertGenericTask{T}"/>
    /// <param name="actual"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertGenericTask<TItem>> Also<TSelf, TItem>(
        this Task<TSelf> origin,
        Task<TItem>? actual
    )
        where TSelf : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual);
    }

    /// <inheritdoc cref="AssertGenericValueTask{T}"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertGenericValueTask<TItem>> Also<TSelf, TItem>(
        this Task<TSelf> origin,
        ValueTask<TItem>? actual
    )
        where TSelf : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual);
    }

    /// <inheritdoc cref="AssertGenericValueTask{T}"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertGenericValueTask<TItem>> Also<TSelf, TItem>(
        this Task<TSelf> origin,
        ValueTask<TItem> actual
    )
        where TSelf : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual);
    }

    /// <inheritdoc cref="AssertTask"/>
    /// <param name="actual"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertTask> Also<T>(this Task<T> origin, Task? actual)
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual);
    }

    /// <inheritdoc cref="AssertValueTask"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertValueTask> Also<T>(this Task<T> origin, ValueTask actual)
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual);
    }

    /// <inheritdoc cref="AssertValueTask"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertValueTask> Also<T>(this Task<T> origin, ValueTask? actual)
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual);
    }

    /// <inheritdoc cref="AssertDelegate"/>
    /// <param name="actual"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertAction> Also<T>(this Task<T> origin, Action? actual)
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual);
    }

    /// <inheritdoc cref="AssertComparable"/>
    /// <param name="actual"><inheritdoc cref="AssertComparableBase{T}.Value" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertComparable> Also<T>(
        this Task<T> origin,
        Func<IComparable?> actual
    )
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual.Invoke());
    }

    /// <inheritdoc cref="AssertEnumerable"/>
    /// <param name="actual"><inheritdoc cref="AssertEnumerableBase{T}.Collection" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertEnumerable> Also<T>(
        this Task<T> origin,
        Func<IEnumerable?> actual
    )
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual.Invoke());
    }

    /// <inheritdoc cref="AssertError"/>
    /// <param name="actual"><inheritdoc cref="AssertErrorBase{T}.Error" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertError> Also<TOrigin>(
        this Task<TOrigin> origin,
        Func<Exception?> actual
    )
        where TOrigin : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual.Invoke());
    }

    /*/// <inheritdoc cref="AssertDelegate"/>
    /// <param name="actual"><inheritdoc cref="AssertDelegateBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertFunc<TItem>> Also<TSelf, TItem>(
        this Task<TSelf> origin,
        Func<TItem>? actual
    )
        where TSelf : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual);
    }*/

    /// <inheritdoc cref="AssertString"/>
    /// <param name="actual"><inheritdoc cref="AssertStringBase{T}.Text" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertString> Also<TOrigin>(
        this Task<TOrigin> origin,
        Func<string?> actual
    )
        where TOrigin : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual.Invoke());
    }

    /// <inheritdoc cref="AssertType"/>
    /// <param name="actual"><inheritdoc cref="AssertTypeBase{T}.Type" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertType> Also<TOrigin>(
        this Task<TOrigin> origin,
        Func<Type?> actual
    )
        where TOrigin : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual.Invoke());
    }
}
