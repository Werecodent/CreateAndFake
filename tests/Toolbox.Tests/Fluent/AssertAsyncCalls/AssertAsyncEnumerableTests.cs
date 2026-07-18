using CreateAndFake.Fluent.AssertAsyncCalls;

namespace CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertAsyncEnumerableTests
{
    [Fact]
    internal static Task AssertAsyncEnumerable_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(AssertAsyncEnumerable<string>),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true, DisableNullRefExceptionTests = true }
        );
    }

    [Fact]
    internal static Task AssertAsyncEnumerable_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(AssertAsyncEnumerable<string>),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true, DisableParameterMutationTests = true }
        );
    }
}
