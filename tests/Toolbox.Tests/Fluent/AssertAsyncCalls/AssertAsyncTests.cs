using CreateAndFake.Fluent.AssertAsyncCalls;

namespace CreateAndFake.Tests.Fluent.AssertAsyncCalls;

public sealed class AssertAsyncTests
{
    [Fact]
    internal static Task AssertAsync_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<AssertAsync>();
    }

    [Fact]
    internal static Task AssertAsync_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<AssertAsync>();
    }

    [Theory, RandomData]
    internal Task Throws_HandlesAsyncNoError(InvalidDataException error)
    {
        return error.Assert(async e => await WaitTest(e)).Throws<InvalidDataException>();
    }

    private static async Task<bool> WaitTest(InvalidDataException error)
    {
        await Task.Delay(0, TestContext.Current.CancellationToken);
        throw error;
    }
}
