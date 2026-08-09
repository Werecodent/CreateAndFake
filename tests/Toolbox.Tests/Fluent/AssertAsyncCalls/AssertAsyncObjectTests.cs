using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;

namespace Werecodent.CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertAsyncObjectTests
{
    [Fact]
    internal static Task AssertAsync_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertAsyncObject>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true }
        );
    }

    [Fact]
    internal static Task AssertAsync_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertAsyncObject>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true }
        );
    }
}
