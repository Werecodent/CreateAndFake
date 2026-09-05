using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

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
    internal static void Also_HandlesObject(AlsoChainer chainer, object data)
    {
        chainer.Also(data).GetType().Assert().Inherits<AssertAsyncObject>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesCollection(AlsoChainer chainer, IEnumerable<int> data)
    {
        chainer.Also(data).GetType().Assert().Inherits<AssertEnumerable>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesString(AlsoChainer chainer, string data)
    {
        chainer.Also(data).GetType().Assert().Inherits<AssertString>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesComparables(AlsoChainer chainer, IComparable data)
    {
        chainer.Also(data).GetType().Assert().Inherits<AssertComparable>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesType(AlsoChainer chainer, Type data)
    {
        chainer.Also(data).GetType().Assert().Inherits<AssertType>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesException(AlsoChainer chainer, Exception data)
    {
        chainer.Also(data).GetType().Assert().Inherits<AssertError>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesAction(AlsoChainer chainer, Action behavior)
    {
        chainer.Also(behavior).GetType().Assert().Inherits<AssertAction>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesFunc(AlsoChainer chainer, Func<string> behavior)
    {
        chainer.Also(behavior).GetType().Assert().Inherits(typeof(AssertFunc<>));
    }

    [Theory, RandomData]
    internal static void Also_HandlesCompiledAction(AlsoChainer chainer, string data)
    {
        chainer
            .Also(() => data.Assert().Called())
            .GetType()
            .Assert()
            .Inherits(typeof(AssertFunc<>));
    }

    [Theory, RandomData]
    internal static void Also_HandlesCompiledFunc(AlsoChainer chainer, string data)
    {
        chainer.Also(() => data.Length).GetType().Assert().Inherits(typeof(AssertFunc<>));
    }
}
