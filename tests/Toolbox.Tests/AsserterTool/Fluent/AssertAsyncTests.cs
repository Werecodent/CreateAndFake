using CreateAndFake.AsserterTool.Fluent;

namespace CreateAndFake.Tests.AsserterTool.Fluent;

public sealed class AssertAsyncTests
{
    [Fact]
    internal static void AssertAsync_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<AssertAsync>();
    }

    [Fact]
    internal static void AssertAsync_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<AssertAsync>();
    }

    [Theory, RandomData]
    internal async Task Throws_HandlesAsyncNoError(InvalidDataException error)
    {
        await error.Assert(async e => await WaitTest(e)).Throws<InvalidDataException>();
    }

    private static async Task<bool> WaitTest(InvalidDataException error)
    {
        await Task.Delay(0, TestContext.Current.CancellationToken);
        throw error;
    }
}
