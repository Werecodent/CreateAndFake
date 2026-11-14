using System.Collections;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskAlsoChainerExtensions
{
    /// <inheritdoc cref="AssertAsyncEnumerable{T}"/>
    /// <param name="collection"><inheritdoc cref="AssertAsyncEnumerableBase{T,T}.Collection" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="collection"/> with.</returns>
    public static async Task<AssertAsyncEnumerable<TItem>> Also<TSelf, TItem>(
        this Task<TSelf> origin,
        Func<IAsyncEnumerable<TItem>> collection
    )
        where TSelf : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, collection);
        return (await origin.ConfigureAwait(false)).Also(collection.Invoke());
    }

    /// <inheritdoc cref="AssertAsyncObject"/>
    /// <param name="actual"><inheritdoc cref="AssertObjectBase{T}.Actual" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="actual"/> with.</returns>
    public static async Task<AssertAsyncObject> Also<T>(this Task<T> origin, Func<object?> actual)
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, actual);
        return (await origin.ConfigureAwait(false)).Also(actual.Invoke());
    }

    /// <inheritdoc cref="AssertGenericTask{T}"/>
    /// <param name="behavior"><inheritdoc cref="AssertGenericTaskBase{T,T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static async Task<AssertGenericTask<TItem>> Also<TSelf, TItem>(
        this Task<TSelf> origin,
        Func<Task<TItem>?> behavior
    )
        where TSelf : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, behavior);
        return (await origin.ConfigureAwait(false)).Also(behavior.Invoke());
    }

    /// <inheritdoc cref="AssertGenericValueTask{T}"/>
    /// <param name="behavior"><inheritdoc cref="AssertGenericValueTaskBase{T,T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static async Task<AssertGenericValueTask<TItem>> Also<TSelf, TItem>(
        this Task<TSelf> origin,
        Func<ValueTask<TItem>?> behavior
    )
        where TSelf : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, behavior);
        return (await origin.ConfigureAwait(false)).Also(behavior.Invoke());
    }

    /// <inheritdoc cref="AssertGenericValueTask{T}"/>
    /// <param name="behavior"><inheritdoc cref="AssertGenericValueTaskBase{T,T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static async Task<AssertGenericValueTask<TItem>> Also<TSelf, TItem>(
        this Task<TSelf> origin,
        Func<ValueTask<TItem>> behavior
    )
        where TSelf : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, behavior);
        return (await origin.ConfigureAwait(false)).Also(behavior.Invoke());
    }

    /// <inheritdoc cref="AssertTask"/>
    /// <param name="behavior"><inheritdoc cref="AssertTaskBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static async Task<AssertTask> Also<T>(this Task<T> origin, Func<Task?> behavior)
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, behavior);
        return (await origin.ConfigureAwait(false)).Also(behavior.Invoke());
    }

    /// <inheritdoc cref="AssertValueTask"/>
    /// <param name="behavior"><inheritdoc cref="AssertValueTaskBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static async Task<AssertValueTask> Also<T>(this Task<T> origin, Func<ValueTask> behavior)
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, behavior);
        return (await origin.ConfigureAwait(false)).Also(behavior.Invoke());
    }

    /// <inheritdoc cref="AssertValueTask"/>
    /// <param name="behavior"><inheritdoc cref="AssertValueTaskBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static async Task<AssertValueTask> Also<T>(
        this Task<T> origin,
        Func<ValueTask?> behavior
    )
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, behavior);
        return (await origin.ConfigureAwait(false)).Also(behavior.Invoke());
    }

    /// <inheritdoc cref="AssertAction"/>
    /// <param name="behavior"><inheritdoc cref="AssertActionBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static async Task<AssertAction> Also<T>(this Task<T> origin, Func<Action?> behavior)
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, behavior);
        return (await origin.ConfigureAwait(false)).Also(behavior.Invoke());
    }

    /// <inheritdoc cref="AssertAction"/>
    /// <param name="behavior"><inheritdoc cref="AssertActionBase{T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static async Task<AssertAction> Also<T>(this Task<T> origin, Action? behavior)
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).Also(behavior);
    }

    /// <inheritdoc cref="AssertComparable"/>
    /// <param name="value"><inheritdoc cref="AssertComparableBase{T}.Value" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="value"/> with.</returns>
    public static async Task<AssertComparable> Also<T>(
        this Task<T> origin,
        Func<IComparable?> value
    )
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, value);
        return (await origin.ConfigureAwait(false)).Also(value.Invoke());
    }

    /// <inheritdoc cref="AssertEnumerable"/>
    /// <param name="collection"><inheritdoc cref="AssertEnumerableBase{T}.Collection" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="collection"/> with.</returns>
    public static async Task<AssertEnumerable> Also<T>(
        this Task<T> origin,
        Func<IEnumerable?> collection
    )
        where T : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, collection);
        return (await origin.ConfigureAwait(false)).Also(collection.Invoke());
    }

    /// <inheritdoc cref="AssertError"/>
    /// <param name="exception"><inheritdoc cref="AssertErrorBase{T}.Error" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="exception"/> with.</returns>
    public static async Task<AssertError> Also<TOrigin>(
        this Task<TOrigin> origin,
        Func<Exception?> exception
    )
        where TOrigin : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, exception);
        return (await origin.ConfigureAwait(false)).Also(exception.Invoke());
    }

    /// <inheritdoc cref="AssertFunc{T}"/>
    /// <param name="behavior"><inheritdoc cref="AssertFuncBase{T,T}.Behavior" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="behavior"/> with.</returns>
    public static async Task<AssertFunc<TItem>> Also<TSelf, TItem>(
        this Task<TSelf> origin,
        Func<Func<TItem>>? behavior
    )
        where TSelf : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, behavior);
        return (await origin.ConfigureAwait(false)).Also(behavior.Invoke());
    }

    /// <inheritdoc cref="AssertString"/>
    /// <param name="text"><inheritdoc cref="AssertStringBase{T}.Text" path="/summary"/></param>
    /// <returns>Asserter to test <paramref name="text"/> with.</returns>
    public static async Task<AssertString> Also<TOrigin>(
        this Task<TOrigin> origin,
        Func<string?> text
    )
        where TOrigin : AlsoChainer
    {
        ArgumentGuard.ThrowIfNull(origin, text);
        return (await origin.ConfigureAwait(false)).Also(text.Invoke());
    }
}
