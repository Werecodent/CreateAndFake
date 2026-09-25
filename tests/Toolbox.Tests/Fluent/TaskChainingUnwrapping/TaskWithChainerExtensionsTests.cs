using System.Collections;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskChainingUnwrapping;

public static class TaskWithChainerExtensionsTests
{
    [Fact]
    internal static Task TaskWithChainerExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskWithChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task TaskWithChainerExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskWithChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Theory, RandomData]
    internal static void With_HandlesCompiledAction(
        Task<ResultChainer<object>> chainer,
        string data
    )
    {
        chainer.With(_ => data.Assert().Fail()).GetType().Assert().Is(typeof(Task<AssertAction>));
    }

    [Theory, RandomData]
    internal static void With_SupportsIAsyncEnumerable(
        Task<ResultChainer<object>> chainer,
        IAsyncEnumerable<int> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertAsyncEnumerable<int>>));
    }

    [Theory, RandomData]
    internal static void With_SupportsSpecificIAsyncEnumerable(
        Task<ResultChainer<object>> chainer,
        AsyncList<string> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertAsyncEnumerable<string>>));
    }

    [Theory, RandomData]
    internal static void With_SupportsObject(Task<ResultChainer<object>> chainer)
    {
        chainer.With(x => x).GetType().Assert().Is(typeof(Task<AssertAsyncObject>));
    }

    [Theory, RandomData]
    internal static void With_SupportsUnknownObject(Task<ResultChainer<DataSample>> chainer)
    {
        chainer.With(x => x).GetType().Assert().Is(typeof(Task<AssertAsyncObject>));
    }

    [Theory, RandomData]
    internal static void With_SupportsGenericTask(
        Task<ResultChainer<object>> chainer,
        Task<string> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertGenericTask<string>>));
    }

    [Theory, RandomData]
    internal static void With_SupportsGenericValueTask(
        Task<ResultChainer<object>> chainer,
        ValueTask<int> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertGenericValueTask<int>>));
    }

    [Theory, RandomData]
    internal static void With_SupportsNullableGenericValueTask(
        Task<ResultChainer<object>> chainer,
        ValueTask<int>? data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertGenericValueTask<int>>));
    }

    [Theory, RandomData]
    internal static void With_SupportsTask(Task<ResultChainer<object>> chainer, Task data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertTask>));
    }

    [Theory, RandomData]
    internal static void With_SupportsValueTask(Task<ResultChainer<object>> chainer, ValueTask data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertValueTask>));
    }

    [Theory, RandomData]
    internal static void With_SupportsNullableValueTask(
        Task<ResultChainer<object>> chainer,
        ValueTask? data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertValueTask>));
    }

    [Theory, RandomData]
    internal static void With_SupportsAction(Task<ResultChainer<object>> chainer, Action data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertAction>));
    }

    [Theory, RandomData]
    internal static void With_SupportsIComparable(
        Task<ResultChainer<object>> chainer,
        IComparable data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void With_SupportsDelegate(Task<ResultChainer<Delegate>> chainer, Delegate data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertDelegate>));
    }

    [Theory, RandomData]
    internal static void With_SupportsInt(Task<ResultChainer<DataSample>> chainer)
    {
        chainer.With(x => x.NumberValue).GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void With_SupportsGenericIEnumerable(Task<ResultChainer<DataSample>> chainer)
    {
        chainer.With(x => x.CollectionValue).GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void With_SupportsList(Task<ResultChainer<List<int>>> chainer)
    {
        chainer.With(x => x).GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void With_SupportsArray(Task<ResultChainer<string[]>> chainer)
    {
        chainer.With(x => x).GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void With_SupportsIEnumerable(
        Task<ResultChainer<object>> chainer,
        IEnumerable data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void With_SupportsException(Task<ResultChainer<Exception>> chainer)
    {
        chainer.With(x => x.InnerException).GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void With_SupportsSpecificException(
        Task<ResultChainer<ArgumentException>> chainer
    )
    {
        chainer.With(x => x.InnerException).GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void With_SupportsFunc(Task<ResultChainer<object>> chainer, Func<int> data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertFunc<int>>));
    }

    [Theory, RandomData]
    internal static void With_SupportsString(Task<ResultChainer<object>> chainer)
    {
        chainer.With(x => x.ToString()).GetType().Assert().Is(typeof(Task<AssertString>));
    }
}
