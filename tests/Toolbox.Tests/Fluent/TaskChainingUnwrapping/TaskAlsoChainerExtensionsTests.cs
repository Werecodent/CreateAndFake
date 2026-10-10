using System.Collections;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.TaskChainingUnwrapping;

public static class TaskAlsoChainerExtensionsTests
{
    [Fact]
    internal static Task TaskAlsoChainerExtensions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(TaskAlsoChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Fact]
    internal static Task TaskAlsoChainerExtensions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(TaskAlsoChainerExtensions),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(AssertException)] }
        );
    }

    [Theory, RandomData]
    internal static void Also_HandlesCompiledAction(Task<AlsoChainer> chainer, string data)
    {
        chainer.Also(() => data.Assert().Fail()).GetType().Assert().Is(typeof(Task<AssertAction>));
    }

    [Theory, RandomData]
    internal static void Also_HandlesCompiledFunc(Task<AlsoChainer> chainer, string data)
    {
        chainer.Also(() => data.Length).GetType().Assert().Inherits(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsIAsyncEnumerable(
        Task<AlsoChainer> chainer,
        IAsyncEnumerable<int> data
    )
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertAsyncEnumerable<int>>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsSpecificIAsyncEnumerable(
        Task<AlsoChainer> chainer,
        AsyncList<string> data
    )
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertAsyncEnumerable<string>>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsObject(Task<AlsoChainer> chainer, object data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertAsyncObject>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsUnknownObject(Task<AlsoChainer> chainer, DataSample data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertAsyncObject>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsGenericTask(Task<AlsoChainer> chainer, Task<string> data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertGenericTask<string>>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsGenericValueTask(
        Task<AlsoChainer> chainer,
        ValueTask<int> data
    )
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertGenericValueTask<int>>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsNullableGenericValueTask(
        Task<AlsoChainer> chainer,
        ValueTask<int>? data
    )
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertGenericValueTask<int>>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsTask(Task<AlsoChainer> chainer, Task data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertTask>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsValueTask(Task<AlsoChainer> chainer, ValueTask data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertValueTask>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsNullableValueTask(Task<AlsoChainer> chainer, ValueTask? data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertValueTask>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsAction(Task<AlsoChainer> chainer, Action data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertAction>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsIComparable(Task<AlsoChainer> chainer, IComparable data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsInt(Task<AlsoChainer> chainer, int data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertComparable>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsGenericIEnumerable(
        Task<AlsoChainer> chainer,
        IEnumerable<string> data
    )
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsList(Task<AlsoChainer> chainer, List<int> data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsArray(Task<AlsoChainer> chainer, string[] data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsIEnumerable(Task<AlsoChainer> chainer, IEnumerable data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertEnumerable>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsException(Task<AlsoChainer> chainer, Exception data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsSpecificException(
        Task<AlsoChainer> chainer,
        ArgumentException data
    )
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertError>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsFunc(Task<AlsoChainer> chainer, Func<int> data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertFunc<int>>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsString(Task<AlsoChainer> chainer, string data)
    {
        chainer.Also(() => data).GetType().Assert().Is(typeof(Task<AssertString>));
    }
}
