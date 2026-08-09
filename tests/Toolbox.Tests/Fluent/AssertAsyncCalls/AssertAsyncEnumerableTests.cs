using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertAsyncEnumerableTests
{
    [Fact]
    internal static Task AssertAsyncEnumerable_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(AssertAsyncEnumerable<>),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true }
        );
    }

    [Fact]
    internal static Task AssertAsyncEnumerable_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(AssertAsyncEnumerable<>),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true }
        );
    }
}
