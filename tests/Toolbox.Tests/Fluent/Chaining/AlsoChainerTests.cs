using System.Collections;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.Chaining;

public static class AlsoChainerTests
{
    [Fact]
    internal static Task AlsoChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AlsoChainer>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AlsoChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AlsoChainer>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void Also_HandlesCompiledAction(AlsoChainer chainer, string data)
    {
        chainer.Also(() => data.Assert().Fail()).GetType().Assert().Is(typeof(AssertAction));
    }

    [Theory, RandomData]
    internal static void Also_HandlesCompiledFunc(AlsoChainer chainer, string data)
    {
        chainer.Also(() => data.Length).GetType().Assert().Inherits(typeof(AssertFunc<int>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsIAsyncEnumerable(
        AlsoChainer chainer,
        IAsyncEnumerable<int> data
    )
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertAsyncEnumerable<int>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsSpecificIAsyncEnumerable(
        AlsoChainer chainer,
        AsyncList<string> data
    )
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertAsyncEnumerable<string>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsObject(AlsoChainer chainer, object data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertAsyncObject));
    }

    [Theory, RandomData]
    internal static void Also_SupportsUnknownObject(AlsoChainer chainer, DataSample data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertAsyncObject));
    }

    [Theory, RandomData]
    internal static void Also_SupportsGenericTask(AlsoChainer chainer, Task<string> data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertGenericTask<string>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsGenericValueTask(AlsoChainer chainer, ValueTask<int> data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertGenericValueTask<int>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsNullableGenericValueTask(
        AlsoChainer chainer,
        ValueTask<int>? data
    )
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertGenericValueTask<int>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsTask(AlsoChainer chainer, Task data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertTask));
    }

    [Theory, RandomData]
    internal static void Also_SupportsValueTask(AlsoChainer chainer, ValueTask data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertValueTask));
    }

    [Theory, RandomData]
    internal static void Also_SupportsNullableValueTask(AlsoChainer chainer, ValueTask? data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertValueTask));
    }

    [Theory, RandomData]
    internal static void Also_SupportsAction(AlsoChainer chainer, Action data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertAction));
    }

    [Theory, RandomData]
    internal static void Also_SupportsIComparable(AlsoChainer chainer, IComparable data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void Also_SupportsInt(AlsoChainer chainer, int data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void Also_SupportsGenericIEnumerable(
        AlsoChainer chainer,
        IEnumerable<string> data
    )
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void Also_SupportsList(AlsoChainer chainer, List<int> data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void Also_SupportsArray(AlsoChainer chainer, DataSample[] data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void Also_SupportsIEnumerable(AlsoChainer chainer, IEnumerable data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void Also_SupportsException(AlsoChainer chainer, Exception data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void Also_SupportsSpecificException(AlsoChainer chainer, ArgumentException data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void Also_SupportsFunc(AlsoChainer chainer, Func<int> data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertFunc<int>));
    }

    [Theory, RandomData]
    internal static void Also_SupportsString(AlsoChainer chainer, string data)
    {
        chainer.Also(data).GetType().Assert().Is(typeof(AssertString));
    }
}
