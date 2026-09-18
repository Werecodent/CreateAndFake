using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Numerics;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Fluent;

/// <summary>Provides fluent assertions.</summary>
public static class TaskResultChainerExtensions
{
    /// <inheritdoc cref="ResultChainer{T}.GetResultValue"/>
    public static async Task<T> GetResultValue<T>(this Task<ResultChainer<T>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).GetResultValue();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertAsyncEnumerable<T>> That<T>(
        this Task<ResultChainer<IAsyncEnumerable<T>>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertAsyncEnumerable<T>> That<T>(
        this Task<ResultChainer<AsyncList<T>>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertAsyncEnumerable<T>> That<T>(
        this Task<ResultChainer<AsyncHashSet<T>>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertAsyncEnumerable<T>> That<T>(
        this Task<ResultChainer<IAsyncSet<T>>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertAsyncObject> That<T>(this Task<ResultChainer<T>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertGenericTask<T>> That<T>(
        this Task<ResultChainer<Task<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertGenericValueTask<T>> That<T>(
        this Task<ResultChainer<ValueTask<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertGenericValueTask<T>> That<T>(
        this Task<ResultChainer<ValueTask<T>>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertTask> That(this Task<ResultChainer<Task?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertValueTask> That(this Task<ResultChainer<ValueTask>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertValueTask> That(this Task<ResultChainer<ValueTask?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertAction> That(this Task<ResultChainer<Action?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<IComparable?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(this Task<ResultChainer<T[]?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(
        this Task<ResultChainer<IEnumerable<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(
        this Task<ResultChainer<ICollection<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(
        this Task<ResultChainer<IReadOnlyCollection<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(this Task<ResultChainer<IList<T>?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(this Task<ResultChainer<List<T>?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(this Task<ResultChainer<Queue<T>?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(this Task<ResultChainer<Stack<T>?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(this Task<ResultChainer<ISet<T>?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(this Task<ResultChainer<HashSet<T>?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(
        this Task<ResultChainer<LinkedList<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(
        this Task<ResultChainer<ConcurrentQueue<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(
        this Task<ResultChainer<ConcurrentStack<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(
        this Task<ResultChainer<FrozenSet<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(
        this Task<ResultChainer<ImmutableList<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(
        this Task<ResultChainer<ImmutableQueue<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(
        this Task<ResultChainer<ImmutableStack<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(
        this Task<ResultChainer<ImmutableHashSet<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<T>(
        this Task<ResultChainer<ImmutableSortedSet<T>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That<TKey, TValue>(
        this Task<ResultChainer<IDictionary<TKey, TValue>?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{TKey,TValue}(Task{ResultChainer{IDictionary{TKey,TValue}}})"/>
    public static async Task<AssertEnumerable> That<TKey, TValue>(
        this Task<ResultChainer<Dictionary<TKey, TValue>?>> origin
    )
        where TKey : notnull
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{TKey,TValue}(Task{ResultChainer{IDictionary{TKey,TValue}}})"/>
    public static async Task<AssertEnumerable> That<TKey, TValue>(
        this Task<ResultChainer<ConcurrentDictionary<TKey, TValue>?>> origin
    )
        where TKey : notnull
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{TKey,TValue}(Task{ResultChainer{IDictionary{TKey,TValue}}})"/>
    public static async Task<AssertEnumerable> That<TKey, TValue>(
        this Task<ResultChainer<FrozenDictionary<TKey, TValue>?>> origin
    )
        where TKey : notnull
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{TKey,TValue}(Task{ResultChainer{IDictionary{TKey,TValue}}})"/>
    public static async Task<AssertEnumerable> That<TKey, TValue>(
        this Task<ResultChainer<ImmutableDictionary<TKey, TValue>?>> origin
    )
        where TKey : notnull
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{TKey,TValue}(Task{ResultChainer{IDictionary{TKey,TValue}}})"/>
    public static async Task<AssertEnumerable> That<TKey, TValue>(
        this Task<ResultChainer<ImmutableSortedDictionary<TKey, TValue>?>> origin
    )
        where TKey : notnull
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(this Task<ResultChainer<IEnumerable?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(this Task<ResultChainer<ICollection?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(this Task<ResultChainer<IDictionary?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(this Task<ResultChainer<Hashtable?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(this Task<ResultChainer<SortedList?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(
        this Task<ResultChainer<ListDictionary?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(
        this Task<ResultChainer<HybridDictionary?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(
        this Task<ResultChainer<StringDictionary?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(
        this Task<ResultChainer<OrderedDictionary?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(
        this Task<ResultChainer<NameValueCollection?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(this Task<ResultChainer<Array?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(this Task<ResultChainer<Stack?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(this Task<ResultChainer<Queue?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(this Task<ResultChainer<ArrayList?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(this Task<ResultChainer<BitArray?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertEnumerable> That(
        this Task<ResultChainer<StringCollection?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(this Task<ResultChainer<Exception?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(this Task<ResultChainer<AggregateException?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(this Task<ResultChainer<ArgumentException?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(
        this Task<ResultChainer<NotSupportedException?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(
        this Task<ResultChainer<InvalidOperationException?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(this Task<ResultChainer<AssertException?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(this Task<ResultChainer<SystemException?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(
        this Task<ResultChainer<TaskCanceledException?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(
        this Task<ResultChainer<OperationCanceledException?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(this Task<ResultChainer<FormatException?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(
        this Task<ResultChainer<ArgumentNullException?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(
        this Task<ResultChainer<NullReferenceException?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(
        this Task<ResultChainer<IndexOutOfRangeException?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(
        this Task<ResultChainer<InvalidCastException?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(
        this Task<ResultChainer<ArgumentOutOfRangeException?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(this Task<ResultChainer<IOException?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(
        this Task<ResultChainer<FileNotFoundException?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(
        this Task<ResultChainer<KeyNotFoundException?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertError> That(
        this Task<ResultChainer<NotImplementedException?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="AlsoChainer.Also{T}(Func{T})"/>
    /// <param name="origin">Assert provider.</param>
    public static async Task<AssertFunc<T>> That<T>(this Task<ResultChainer<Func<T>?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertString> That(this Task<ResultChainer<string?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<bool>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<bool?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<float>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<float?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<double>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<double?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<decimal>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<decimal?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<TimeSpan>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<TimeSpan?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<DateTime>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<DateTime?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<BigInteger>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<BigInteger?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<DateTimeOffset>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(
        this Task<ResultChainer<DateTimeOffset?>> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<char>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<char?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<int>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<int?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<long>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<long?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<short>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<short?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<byte>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }

    /// <inheritdoc cref="That{T}(Task{ResultChainer{Func{T}}})"/>
    public static async Task<AssertComparable> That(this Task<ResultChainer<byte?>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return (await origin.ConfigureAwait(false)).That();
    }
}
