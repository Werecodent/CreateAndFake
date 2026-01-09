using CreateAndFake.Fluent.AssertAsyncCalls;

namespace CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertAsyncObjectTests
{
    [Fact]
    internal static Task AssertAsync_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<AssertAsyncObject>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true }
        );
    }

    [Fact]
    internal static Task AssertAsync_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<AssertAsyncObject>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true }
        );
    }
}
