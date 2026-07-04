using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RunnerTool;

public static class RunResultsTests
{
    [Fact]
    internal static Task RunResults_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<RunResults>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task RunResults_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<RunResults>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static void ToString_Debug(RunResults results)
    {
        results.ToString().Assert().Debug();
    }
}
