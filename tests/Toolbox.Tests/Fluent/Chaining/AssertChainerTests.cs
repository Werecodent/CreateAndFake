using Werecodent.CreateAndFake.Fluent.Chaining;

namespace Werecodent.CreateAndFake.Tests.Fluent.Chaining;

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
        new AssertChainer<object>(data, Tools.Asserter).And().Assert().Is(data);
    }
}
