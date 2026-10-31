using System.Collections;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.Fluent.Chaining;

public static class ResultChainerTests
{
    [Fact]
    internal static Task ResultChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ResultChainer<>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ResultChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(ResultChainer<>),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void With_HandlesCompiledAction(ResultChainer<object> chainer, string data)
    {
        chainer.With(_ => data.Assert().Fail()).GetType().Assert().Is(typeof(AssertAction));
    }

    [Theory, RandomData]
    internal static void With_SupportsIAsyncEnumerable(
        ResultChainer<object> chainer,
        IAsyncEnumerable<int> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(AssertAsyncEnumerable<int>));
    }

    [Theory, RandomData]
    internal static void With_SupportsSpecificIAsyncEnumerable(
        ResultChainer<object> chainer,
        AsyncList<string> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(AssertAsyncEnumerable<string>));
    }

    [Theory, RandomData]
    internal static void With_SupportsObject(ResultChainer<object> chainer)
    {
        chainer.With(x => x).GetType().Assert().Is(typeof(AssertAsyncObject));
    }

    [Theory, RandomData]
    internal static void With_SupportsUnknownObject(ResultChainer<DataSample> chainer)
    {
        chainer.With(x => x).GetType().Assert().Is(typeof(AssertAsyncObject));
    }

    [Theory, RandomData]
    internal static void With_SupportsGenericTask(ResultChainer<object> chainer, Task<string> data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(AssertGenericTask<string>));
    }

    [Theory, RandomData]
    internal static void With_SupportsGenericValueTask(
        ResultChainer<object> chainer,
        ValueTask<int> data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(AssertGenericValueTask<int>));
    }

    [Theory, RandomData]
    internal static void With_SupportsNullableGenericValueTask(
        ResultChainer<object> chainer,
        ValueTask<int>? data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(AssertGenericValueTask<int>));
    }

    [Theory, RandomData]
    internal static void With_SupportsTask(ResultChainer<object> chainer, Task data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(AssertTask));
    }

    [Theory, RandomData]
    internal static void With_SupportsValueTask(ResultChainer<object> chainer, ValueTask data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(AssertValueTask));
    }

    [Theory, RandomData]
    internal static void With_SupportsNullableValueTask(
        ResultChainer<object> chainer,
        ValueTask? data
    )
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(AssertValueTask));
    }

    [Theory, RandomData]
    internal static void With_SupportsAction(ResultChainer<object> chainer, Action data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(AssertAction));
    }

    [Theory, RandomData]
    internal static void With_SupportsIComparable(ResultChainer<object> chainer, IComparable data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void With_SupportsDelegate(ResultChainer<object> chainer, Delegate data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(AssertDelegate));
    }

    [Theory, RandomData]
    internal static void With_SupportsInt(ResultChainer<DataSample> chainer)
    {
        chainer.With(x => x.NumberValue).GetType().Assert().Is(typeof(AssertComparable));
    }

    [Theory, RandomData]
    internal static void With_SupportsGenericIEnumerable(ResultChainer<DataSample> chainer)
    {
        chainer.With(x => x.CollectionValue).GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void With_SupportsList(ResultChainer<List<int>> chainer)
    {
        chainer.With(x => x).GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void With_SupportsArray(ResultChainer<string[]> chainer)
    {
        chainer.With(x => x).GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void With_SupportsIEnumerable(ResultChainer<object> chainer, IEnumerable data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(AssertEnumerable));
    }

    [Theory, RandomData]
    internal static void With_SupportsException(ResultChainer<Exception> chainer)
    {
        chainer.With(x => x.InnerException).GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void With_SupportsSpecificException(ResultChainer<ArgumentException> chainer)
    {
        chainer.With(x => x).GetType().Assert().Is(typeof(AssertError));
    }

    [Theory, RandomData]
    internal static void With_SupportsFunc(ResultChainer<object> chainer, Func<int> data)
    {
        chainer.With(_ => data).GetType().Assert().Is(typeof(AssertFunc<int>));
    }

    [Theory, RandomData]
    internal static void With_SupportsString(ResultChainer<object> chainer)
    {
        chainer.With(x => x.ToString()).GetType().Assert().Is(typeof(AssertString));
    }
}
