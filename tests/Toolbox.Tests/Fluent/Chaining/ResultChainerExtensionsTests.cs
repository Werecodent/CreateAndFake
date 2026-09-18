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

namespace Werecodent.CreateAndFake.Tests.Fluent.Chaining;

public static class ResultChainerExtensionsTests
{
    [Fact]
    internal static Task ResultChainerExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ResultChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task ResultChainerExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(ResultChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Theory, RandomData]
    internal static void That_SupportsIAsyncEnumerable(ResultChainer<IAsyncEnumerable<int>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertAsyncEnumerable<int>));
    }

    [Theory, RandomData]
    internal static void That_SupportsAsyncList(ResultChainer<AsyncList<string>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertAsyncEnumerable<string>));
    }

    [Theory, RandomData]
    internal static void That_SupportsIAsyncSet(ResultChainer<IAsyncSet<DataSample>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertAsyncEnumerable<DataSample>));
    }

    [Theory, RandomData]
    internal static void That_SupportsAsyncHashSet(
        ResultChainer<AsyncHashSet<AsyncDataSample>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertAsyncEnumerable<AsyncDataSample>));
    }

    [Theory, RandomData]
    internal static void That_SupportsObject(ResultChainer<object> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertAsyncObject));
    }

    [Theory, RandomData]
    internal static void That_SupportsUnknownObject(ResultChainer<DataSample> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertAsyncObject));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericTask(ResultChainer<Task<string>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertGenericTask<string>));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericValueTask(ResultChainer<ValueTask<int>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertGenericValueTask<int>));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableGenericValueTask(
        ResultChainer<ValueTask<int>?> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertGenericValueTask<int>));
    }

    [Theory, RandomData]
    internal static void That_SupportsTask(ResultChainer<Task> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertTask));
    }

    [Theory, RandomData]
    internal static void That_SupportsValueTask(ResultChainer<ValueTask> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertValueTask));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableValueTask(ResultChainer<ValueTask?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertValueTask));
    }

    [Theory, RandomData]
    internal static void That_SupportsAction(ResultChainer<Action> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertAction));
    }

    [Theory, RandomData]
    internal static void That_SupportsIComparable(ResultChainer<IComparable> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericIEnumerable(ResultChainer<IEnumerable<string>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericArray(ResultChainer<DataSample[]> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericICollection(ResultChainer<ICollection<string>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericIReadOnlyCollection(
        ResultChainer<IReadOnlyCollection<int>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericIList(ResultChainer<IList<string>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericList(ResultChainer<List<string>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericQueue(ResultChainer<Queue<string>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericStack(ResultChainer<Stack<string>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericISet(ResultChainer<ISet<string>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericHashSet(ResultChainer<HashSet<string>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericLinkedList(
        ResultChainer<LinkedList<DataSample>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericConcurrentQueue(
        ResultChainer<ConcurrentQueue<string>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericConcurrentStack(
        ResultChainer<ConcurrentStack<string>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericFrozenSet(ResultChainer<FrozenSet<string>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableList(
        ResultChainer<ImmutableList<string>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableQueue(
        ResultChainer<ImmutableQueue<string>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableStack(
        ResultChainer<ImmutableStack<string>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableHashSet(
        ResultChainer<ImmutableHashSet<string>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableSortedSet(
        ResultChainer<ImmutableSortedSet<string>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericIDictionary(
        ResultChainer<IDictionary<int, string>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericDictionary(
        ResultChainer<Dictionary<string, string>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericConcurrentDictionary(
        ResultChainer<ConcurrentDictionary<int, DataSample>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericFrozenDictionary(
        ResultChainer<FrozenDictionary<int, string>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableDictionary(
        ResultChainer<ImmutableDictionary<int, string>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsGenericImmutableSortedDictionary(
        ResultChainer<ImmutableSortedDictionary<int, string>> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsIEnumerable(ResultChainer<IEnumerable> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsICollection(ResultChainer<ICollection> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsIDictionary(ResultChainer<IDictionary> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsHashtable(ResultChainer<Hashtable> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsSortedList(ResultChainer<SortedList> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsListDictionary(ResultChainer<ListDictionary> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsHybridDictionary(ResultChainer<HybridDictionary> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsStringDictionary(ResultChainer<StringDictionary> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsOrderedDictionary(ResultChainer<OrderedDictionary> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNameValueCollection(
        ResultChainer<NameValueCollection> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsArray(ResultChainer<Array> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsStack(ResultChainer<Stack> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsQueue(ResultChainer<Queue> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsArrayList(ResultChainer<ArrayList> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsBitArray(ResultChainer<BitArray> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsStringCollection(ResultChainer<StringCollection> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void That_SupportsException(ResultChainer<Exception> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsAggregateException(ResultChainer<AggregateException> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsArgumentException(ResultChainer<ArgumentException> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsNotSupportedException(
        ResultChainer<NotSupportedException> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsInvalidOperationException(
        ResultChainer<InvalidOperationException> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsAssertException(ResultChainer<AssertException> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsSystemException(ResultChainer<SystemException> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsTaskCanceledException(
        ResultChainer<TaskCanceledException> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsOperationCanceledException(
        ResultChainer<OperationCanceledException> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsFormatException(ResultChainer<FormatException> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsArgumentNullException(
        ResultChainer<ArgumentNullException> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullReferenceException(
        ResultChainer<NullReferenceException> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsIndexOutOfRangeException(
        ResultChainer<IndexOutOfRangeException> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsInvalidCastException(
        ResultChainer<InvalidCastException> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsArgumentOutOfRangeException(
        ResultChainer<ArgumentOutOfRangeException> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsIOException(ResultChainer<IOException> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsFileNotFoundException(
        ResultChainer<FileNotFoundException> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsKeyNotFoundException(
        ResultChainer<KeyNotFoundException> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsNotImplementedException(
        ResultChainer<NotImplementedException> chainer
    )
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void That_SupportsFunc(ResultChainer<Func<int>> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertFunc<int>));
    }

    [Theory, RandomData]
    internal static void That_SupportsString(ResultChainer<string> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertString));
    }

    [Theory, RandomData]
    internal static void That_SupportsBool(ResultChainer<bool> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableBool(ResultChainer<bool?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsFloat(ResultChainer<float> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableFloat(ResultChainer<float?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsDouble(ResultChainer<double> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableDouble(ResultChainer<double?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsDecimal(ResultChainer<decimal> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableDecimal(ResultChainer<decimal?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsTimeSpan(ResultChainer<TimeSpan> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableTimeSpan(ResultChainer<TimeSpan?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsDateTime(ResultChainer<DateTime> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableDateTime(ResultChainer<DateTime?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsBigInteger(ResultChainer<BigInteger> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableBigInteger(ResultChainer<BigInteger?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsDateTimeOffset(ResultChainer<DateTimeOffset> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableDateTimeOffset(ResultChainer<DateTimeOffset?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsChar(ResultChainer<char> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableChar(ResultChainer<char?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsInt(ResultChainer<int> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableInt(ResultChainer<int?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsLong(ResultChainer<long> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableLong(ResultChainer<long?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsShort(ResultChainer<short> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableShort(ResultChainer<short?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsByte(ResultChainer<byte> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void That_SupportsNullableByte(ResultChainer<byte?> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertComparable));
    }
}
