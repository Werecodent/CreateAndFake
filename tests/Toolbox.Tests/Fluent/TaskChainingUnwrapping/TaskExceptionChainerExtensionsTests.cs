using System.Collections;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskChainingUnwrapping;

public static class TaskExceptionChainerExtensionsTests
{
    [Fact]
    internal static Task TaskExceptionChainerExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskExceptionChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task TaskExceptionChainerExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskExceptionChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Theory, RandomData]
    internal static void With_HandlesCompiledAction(
        Task<ExceptionChainer<Exception>> chainer,
        string data
    )
    {
        chainer.With(_ => data.Assert().Fail()).GetType().Assert().Is(typeof(Task<AssertAction>));
    }

    [Theory, RandomData]
    internal static void With_SupportsIAsyncEnumerable(
        Task<ExceptionChainer<Exception>> chainer,
        IAsyncEnumerable<int> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertAsyncEnumerable<int>>));
    }

    [Theory, RandomData]
    internal static void With_SupportsSpecificIAsyncEnumerable(
        Task<ExceptionChainer<Exception>> chainer,
        AsyncList<string> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertAsyncEnumerable<string>>));
    }

    [Theory, RandomData]
    internal static void With_SupportsObject(Task<ExceptionChainer<Exception>> chainer, object data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertAsyncObject>));
    }

    [Theory, RandomData]
    internal static void With_SupportsUnknownObject(
        Task<ExceptionChainer<Exception>> chainer,
        DataSample data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertAsyncObject>));
    }

    [Theory, RandomData]
    internal static void With_SupportsGenericTask(
        Task<ExceptionChainer<Exception>> chainer,
        Task<string> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertGenericTask<string>>));
    }

    [Theory, RandomData]
    internal static void With_SupportsGenericValueTask(
        Task<ExceptionChainer<Exception>> chainer,
        ValueTask<int> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertGenericValueTask<int>>));
    }

    [Theory, RandomData]
    internal static void With_SupportsNullableGenericValueTask(
        Task<ExceptionChainer<Exception>> chainer,
        ValueTask<int>? data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertGenericValueTask<int>>));
    }

    [Theory, RandomData]
    internal static void With_SupportsTask(Task<ExceptionChainer<Exception>> chainer, Task data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertTask>));
    }

    [Theory, RandomData]
    internal static void With_SupportsValueTask(
        Task<ExceptionChainer<Exception>> chainer,
        ValueTask data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertValueTask>));
    }

    [Theory, RandomData]
    internal static void With_SupportsNullableValueTask(
        Task<ExceptionChainer<Exception>> chainer,
        ValueTask? data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertValueTask>));
    }

    [Theory, RandomData]
    internal static void With_SupportsAction(Task<ExceptionChainer<Exception>> chainer, Action data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertAction>));
    }

    [Theory, RandomData]
    internal static void With_SupportsIComparable(
        Task<ExceptionChainer<Exception>> chainer,
        IComparable data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void With_SupportsInt(Task<ExceptionChainer<Exception>> chainer, int data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void With_SupportsGenericIEnumerable(
        Task<ExceptionChainer<Exception>> chainer,
        IEnumerable<string> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void With_SupportsList(
        Task<ExceptionChainer<Exception>> chainer,
        List<int> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void With_SupportsArray(
        Task<ExceptionChainer<Exception>> chainer,
        DataSample[] data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void With_SupportsIEnumerable(
        Task<ExceptionChainer<Exception>> chainer,
        IEnumerable data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void With_SupportsException(Task<ExceptionChainer<Exception>> chainer)
    {
        chainer.With(x => x.InnerException).GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void With_SupportsSpecificException(
        Task<ExceptionChainer<ArgumentException>> chainer
    )
    {
        chainer.With(x => x).GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void With_SupportsFunc(
        Task<ExceptionChainer<Exception>> chainer,
        Func<int> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(Task<AssertFunc<int>>));
    }

    [Theory, RandomData]
    internal static void With_SupportsString(Task<ExceptionChainer<Exception>> chainer)
    {
        chainer.With(x => x.ToString()).GetType().Assert().Is(typeof(Task<AssertString>));
    }
}
