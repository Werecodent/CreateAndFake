using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RunnerTool;

public static class RunResultTests
{
    [Fact]
    internal static void RunResult_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<RunResult>();
    }

    [Fact]
    internal static void RunResult_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<RunResult>();
    }
}
