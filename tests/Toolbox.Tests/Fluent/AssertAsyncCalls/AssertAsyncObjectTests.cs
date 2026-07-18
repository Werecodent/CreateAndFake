using CreateAndFake.Fluent.AssertAsyncCalls;

namespace CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertAsyncObjectTests
{
    [Fact]
    internal static Task AssertAsync_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertAsyncObject>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true, DisableNullRefExceptionTests = true }
        );
    }

    [Fact]
    internal static Task AssertAsync_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertAsyncObject>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true, DisableParameterMutationTests = true }
        );
    }
}
