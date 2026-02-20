using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RunnerTool;

public static class RunResultsTests
{
    [Fact]
    internal static Task RunResults_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<RunResults>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task RunResults_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<RunResults>(
            TestContext.Current.CancellationToken
        );
    }
}
