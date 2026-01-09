using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RunnerTool;

public static class RunResultTests
{
    [Fact]
    internal static Task RunResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<RunResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task RunResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<RunResult>(
            TestContext.Current.CancellationToken
        );
    }
}
