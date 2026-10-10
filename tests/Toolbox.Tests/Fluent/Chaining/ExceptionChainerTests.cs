using Werecodent.CreateAndFake.Fluent.AssertCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Tests.Fluent.Chaining;

public static class ExceptionChainerTests
{
    [Fact]
    internal static Task ExceptionChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ExceptionChainer<>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ExceptionChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(ExceptionChainer<>),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void That_SupportsArgumentException(ExceptionChainer<ArgumentException> chainer)
    {
        chainer.That().GetType().Assert().Is(typeof(AssertError));
    }
}
