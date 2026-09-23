using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Numerics;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskChainingUnwrapping;

public static class TaskResultChainerExtensionsTests
{
    [Fact]
    internal static Task TaskResultChainerExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskResultChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task TaskResultChainerExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskResultChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Theory, RandomData]
    internal static void That_SupportsIAsyncEnumerable(
        Task<ResultChainer<IAsyncEnumerable<int>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertAsyncEnumerable<int>>));
    }

    [Theory, RandomData]
    internal static void That_SupportsAsyncList(Task<ResultChainer<AsyncList<string>>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertAsyncEnumerable<string>>));
    }

    [Theory, RandomData]
    internal static void That_SupportsIAsyncSet(Task<ResultChainer<IAsyncSet<DataSample>>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertAsyncEnumerable<DataSample>>));
    }

    [Theory, RandomData]
    internal static void That_SupportsAsyncHashSet(
        Task<ResultChainer<AsyncHashSet<AsyncDataSample>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertAsyncEnumerable<AsyncDataSample>>));
    }

    [Theory, RandomData]
    internal static void That_SupportsObject(Task<ResultChainer<object>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertAsyncObject>));
    }

    [Theory, RandomData]
    internal static void That_SupportsUnknownObject(Task<ResultChainer<DataSample>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertAsyncObject>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericTask(Task<ResultChainer<Task<string>>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertGenericTask<string>>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericValueTask(Task<ResultChainer<ValueTask<int>>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertGenericValueTask<int>>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableGenericValueTask(
        Task<ResultChainer<ValueTask<int>?>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertGenericValueTask<int>>));
    }

    [Theory, RandomData]
    internal static void That_SupportsTask(Task<ResultChainer<Task>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertTask>));
    }

    [Theory, RandomData]
    internal static void That_SupportsValueTask(Task<ResultChainer<ValueTask>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertValueTask>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableValueTask(Task<ResultChainer<ValueTask?>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertValueTask>));
    }

    [Theory, RandomData]
    internal static void That_SupportsAction(Task<ResultChainer<Action>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertAction>));
    }

    [Theory, RandomData]
    internal static void That_SupportsIComparable(Task<ResultChainer<IComparable>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsDelegate(Task<ResultChainer<Delegate>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertDelegate>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericIEnumerable(
        Task<ResultChainer<IEnumerable<string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericArray(Task<ResultChainer<DataSample[]>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericICollection(
        Task<ResultChainer<ICollection<string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericIReadOnlyCollection(
        Task<ResultChainer<IReadOnlyCollection<int>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericIList(Task<ResultChainer<IList<string>>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericList(Task<ResultChainer<List<string>>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericQueue(Task<ResultChainer<Queue<string>>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericStack(Task<ResultChainer<Stack<string>>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericISet(Task<ResultChainer<ISet<string>>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericHashSet(Task<ResultChainer<HashSet<string>>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericLinkedList(
        Task<ResultChainer<LinkedList<DataSample>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericConcurrentQueue(
        Task<ResultChainer<ConcurrentQueue<string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericConcurrentStack(
        Task<ResultChainer<ConcurrentStack<string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericFrozenSet(
        Task<ResultChainer<FrozenSet<string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableList(
        Task<ResultChainer<ImmutableList<string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableQueue(
        Task<ResultChainer<ImmutableQueue<string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableStack(
        Task<ResultChainer<ImmutableStack<string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableHashSet(
        Task<ResultChainer<ImmutableHashSet<string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableSortedSet(
        Task<ResultChainer<ImmutableSortedSet<string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericIDictionary(
        Task<ResultChainer<IDictionary<int, string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericDictionary(
        Task<ResultChainer<Dictionary<string, string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericConcurrentDictionary(
        Task<ResultChainer<ConcurrentDictionary<int, DataSample>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericFrozenDictionary(
        Task<ResultChainer<FrozenDictionary<int, string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableDictionary(
        Task<ResultChainer<ImmutableDictionary<int, string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableSortedDictionary(
        Task<ResultChainer<ImmutableSortedDictionary<int, string>>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsIEnumerable(Task<ResultChainer<IEnumerable>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsICollection(Task<ResultChainer<ICollection>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsIDictionary(Task<ResultChainer<IDictionary>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsHashtable(Task<ResultChainer<Hashtable>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsSortedList(Task<ResultChainer<SortedList>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsListDictionary(Task<ResultChainer<ListDictionary>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsHybridDictionary(
        Task<ResultChainer<HybridDictionary>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsStringDictionary(
        Task<ResultChainer<StringDictionary>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsOrderedDictionary(
        Task<ResultChainer<OrderedDictionary>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNameValueCollection(
        Task<ResultChainer<NameValueCollection>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsArray(Task<ResultChainer<Array>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsStack(Task<ResultChainer<Stack>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsQueue(Task<ResultChainer<Queue>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsArrayList(Task<ResultChainer<ArrayList>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsBitArray(Task<ResultChainer<BitArray>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsStringCollection(
        Task<ResultChainer<StringCollection>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsException(Task<ResultChainer<Exception>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsAggregateException(
        Task<ResultChainer<AggregateException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsArgumentException(
        Task<ResultChainer<ArgumentException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNotSupportedException(
        Task<ResultChainer<NotSupportedException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsInvalidOperationException(
        Task<ResultChainer<InvalidOperationException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsAssertException(Task<ResultChainer<AssertException>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsSystemException(Task<ResultChainer<SystemException>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsTaskCanceledException(
        Task<ResultChainer<TaskCanceledException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsOperationCanceledException(
        Task<ResultChainer<OperationCanceledException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsFormatException(Task<ResultChainer<FormatException>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsArgumentNullException(
        Task<ResultChainer<ArgumentNullException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullReferenceException(
        Task<ResultChainer<NullReferenceException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsIndexOutOfRangeException(
        Task<ResultChainer<IndexOutOfRangeException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsInvalidCastException(
        Task<ResultChainer<InvalidCastException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsArgumentOutOfRangeException(
        Task<ResultChainer<ArgumentOutOfRangeException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsIOException(Task<ResultChainer<IOException>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsFileNotFoundException(
        Task<ResultChainer<FileNotFoundException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsKeyNotFoundException(
        Task<ResultChainer<KeyNotFoundException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNotImplementedException(
        Task<ResultChainer<NotImplementedException>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void That_SupportsFunc(Task<ResultChainer<Func<int>>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertFunc<int>>));
    }

    [Theory, RandomData]
    internal static void That_SupportsString(Task<ResultChainer<string>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertString>));
    }

    [Theory, RandomData]
    internal static void That_SupportsBool(Task<ResultChainer<bool>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableBool(Task<ResultChainer<bool?>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsFloat(Task<ResultChainer<float>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableFloat(Task<ResultChainer<float?>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsDouble(Task<ResultChainer<double>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableDouble(Task<ResultChainer<double?>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsDecimal(Task<ResultChainer<decimal>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableDecimal(Task<ResultChainer<decimal?>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsTimeSpan(Task<ResultChainer<TimeSpan>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableTimeSpan(Task<ResultChainer<TimeSpan?>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsDateTime(Task<ResultChainer<DateTime>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableDateTime(Task<ResultChainer<DateTime?>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsBigInteger(Task<ResultChainer<BigInteger>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableBigInteger(Task<ResultChainer<BigInteger?>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsDateTimeOffset(Task<ResultChainer<DateTimeOffset>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableDateTimeOffset(
        Task<ResultChainer<DateTimeOffset?>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsChar(Task<ResultChainer<char>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableChar(Task<ResultChainer<char?>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsInt(Task<ResultChainer<int>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableInt(Task<ResultChainer<int?>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsLong(Task<ResultChainer<long>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableLong(Task<ResultChainer<long?>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsShort(Task<ResultChainer<short>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableShort(Task<ResultChainer<short?>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsByte(Task<ResultChainer<byte>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableByte(Task<ResultChainer<byte?>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(Task<AssertComparable>));
    }
}
