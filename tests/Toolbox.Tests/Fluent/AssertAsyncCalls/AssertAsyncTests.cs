using CreateAndFake.Fluent.AssertAsyncCalls;

namespace CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public static class AssertAsyncTests
{
    [Fact]
    internal static Task AssertAsync_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AssertAsync>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true }
        );
    }

    [Fact]
    internal static Task AssertAsync_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AssertAsync>(
            TestContext.Current.CancellationToken,
            opt => opt with { IgnoreAllExceptions = true }
        );
    }

    [Theory, RandomData]
    internal static Task ThrowsAsync_HandlesAsyncNoError(InvalidDataException error)
    {
        return WaitTest(error)
            .Assert()
            .ThrowsAsync<InvalidDataException>(TestContext.Current.CancellationToken);
    }

    private static async Task<bool> WaitTest(InvalidDataException error)
    {
        await Task.Delay(0, TestContext.Current.CancellationToken);
        throw error;
    }
}
