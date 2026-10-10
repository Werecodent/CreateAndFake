using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.RunnerTool;

public static class RunResultsTests
{
    [Theory, RandomData]
    internal static void Debug_RunResults_ToString(RunResults results)
    {
        results.ToString().Assert().Debug();
    }

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
}
