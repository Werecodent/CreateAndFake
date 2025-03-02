using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RunnerTool;

public static class RunResultsTests
{
    [Fact]
    internal static void RunResults_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<RunResults>();
    }

    [Fact]
    internal static void RunResults_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<RunResults>();
    }
}