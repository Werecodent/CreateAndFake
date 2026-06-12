using CreateAndFake.Fluent.AssertCalls;
using CreateAndFake.Fluent.Chaining;

namespace CreateAndFake.Tests.Fluent.Chaining;

public static class AssertChainerTests
{
    [Fact]
    internal static Task AssertChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertChainer<object>>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AssertChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertChainer<object>>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void And_ReturnsInput(object data)
    {
        new AssertChainer<object>(data, Tools.Asserter).And.Assert().Is(data);
    }

    [Theory, RandomData]
    internal static void Also_HandlesObject(AssertChainer<object> chainer, object data)
    {
        chainer.Also(data).GetType().Assert().Inherits<AssertObject>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesCollection(
        AssertChainer<object> chainer,
        IEnumerable<int> data
    )
    {
        chainer.Also(data).GetType().Assert().Inherits<AssertEnumerable>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesString(AssertChainer<object> chainer, string data)
    {
        chainer.Also(data).GetType().Assert().Inherits<AssertString>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesComparables(AssertChainer<object> chainer, IComparable data)
    {
        chainer.Also(data).GetType().Assert().Inherits<AssertComparable>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesType(AssertChainer<object> chainer, Type data)
    {
        chainer.Also(data).GetType().Assert().Inherits<AssertType>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesException(AssertChainer<object> chainer, Exception data)
    {
        chainer.Also(data).GetType().Assert().Inherits<AssertError>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesAction(AssertChainer<object> chainer, Action behavior)
    {
        chainer.Also(behavior).GetType().Assert().Inherits<AssertDelegate>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesFunc(AssertChainer<object> chainer, Func<string> behavior)
    {
        chainer.Also(behavior).GetType().Assert().Inherits<AssertDelegate>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesCompiledAction(AssertChainer<object> chainer, string data)
    {
        chainer.Also(data, d => d.Assert().Called()).GetType().Assert().Inherits<AssertDelegate>();
    }

    [Theory, RandomData]
    internal static void Also_HandlesCompiledFunc(AssertChainer<object> chainer, string data)
    {
        chainer.Also(data, d => d.Length).GetType().Assert().Inherits<AssertDelegate>();
    }
}
