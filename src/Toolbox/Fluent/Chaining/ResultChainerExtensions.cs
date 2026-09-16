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

#pragma warning disable IDE0130 // Cleaner project organization.

namespace Werecodent.CreateAndFake.Fluent;

#pragma warning restore

/// <summary>Provides fluent assertions.</summary>
public static class ResultChainerExtensions
{
    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertAsyncEnumerable<T> That<T>(this ResultChainer<IAsyncEnumerable<T>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertAsyncEnumerable<T> That<T>(this ResultChainer<AsyncList<T>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertAsyncEnumerable<T> That<T>(this ResultChainer<AsyncHashSet<T>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertAsyncEnumerable<T> That<T>(this ResultChainer<IAsyncSet<T>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertAsyncObject That<T>(this ResultChainer<T> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertGenericTask<T> That<T>(this ResultChainer<Task<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertGenericValueTask<T> That<T>(this ResultChainer<ValueTask<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertGenericValueTask<T> That<T>(this ResultChainer<ValueTask<T>> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertTask That(this ResultChainer<Task?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertValueTask That(this ResultChainer<ValueTask> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertValueTask That(this ResultChainer<ValueTask?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertAction That(this ResultChainer<Action?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<IComparable?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<T[]?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<IEnumerable<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<ICollection<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<IReadOnlyCollection<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<IList<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<List<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<Queue<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<Stack<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<ISet<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<HashSet<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<LinkedList<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<ConcurrentQueue<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<ConcurrentStack<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<FrozenSet<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<ImmutableList<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<ImmutableQueue<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<ImmutableStack<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<ImmutableHashSet<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<T>(this ResultChainer<ImmutableSortedSet<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That<TKey, TValue>(
        this ResultChainer<IDictionary<TKey, TValue>?> origin
    )
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{TKey,TValue}(ResultChainer{IDictionary{TKey,TValue}})"/>
    public static AssertEnumerable That<TKey, TValue>(
        this ResultChainer<Dictionary<TKey, TValue>?> origin
    )
        where TKey : notnull
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{TKey,TValue}(ResultChainer{IDictionary{TKey,TValue}})"/>
    public static AssertEnumerable That<TKey, TValue>(
        this ResultChainer<ConcurrentDictionary<TKey, TValue>?> origin
    )
        where TKey : notnull
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{TKey,TValue}(ResultChainer{IDictionary{TKey,TValue}})"/>
    public static AssertEnumerable That<TKey, TValue>(
        this ResultChainer<FrozenDictionary<TKey, TValue>?> origin
    )
        where TKey : notnull
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{TKey,TValue}(ResultChainer{IDictionary{TKey,TValue}})"/>
    public static AssertEnumerable That<TKey, TValue>(
        this ResultChainer<ImmutableDictionary<TKey, TValue>?> origin
    )
        where TKey : notnull
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{TKey,TValue}(ResultChainer{IDictionary{TKey,TValue}})"/>
    public static AssertEnumerable That<TKey, TValue>(
        this ResultChainer<ImmutableSortedDictionary<TKey, TValue>?> origin
    )
        where TKey : notnull
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<IEnumerable?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<ICollection?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<IDictionary?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<Hashtable?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<SortedList?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<ListDictionary?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<HybridDictionary?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<StringDictionary?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<OrderedDictionary?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<NameValueCollection?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<Array?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<Stack?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<Queue?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<ArrayList?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<BitArray?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertEnumerable That(this ResultChainer<StringCollection?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<Exception?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<AggregateException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<ArgumentException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<NotSupportedException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<InvalidOperationException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<AssertException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<SystemException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<TaskCanceledException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<OperationCanceledException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<FormatException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<ArgumentNullException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<NullReferenceException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<IndexOutOfRangeException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<InvalidCastException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<ArgumentOutOfRangeException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<IOException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<FileNotFoundException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<KeyNotFoundException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertError That(this ResultChainer<NotImplementedException?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="AlsoChainer.Also{T}(Func{T})"/>
    /// <param name="origin">Assert provider.</param>
    public static AssertFunc<T> That<T>(this ResultChainer<Func<T>?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertString That(this ResultChainer<string?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<bool> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<bool?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<float> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<float?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<double> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<double?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<decimal> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<decimal?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<TimeSpan> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<TimeSpan?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<DateTime> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<DateTime?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<BigInteger> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<BigInteger?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<DateTimeOffset> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<DateTimeOffset?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<char> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<char?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<int> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<int?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<long> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<long?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<short> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<short?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<byte> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }

    /// <inheritdoc cref="That{T}(ResultChainer{Func{T}})"/>
    public static AssertComparable That(this ResultChainer<byte?> origin)
    {
        ArgumentGuard.ThrowIfNull(origin);
        return origin.Also(origin.GetResultValue());
    }
}
