global using RunnerMod = System.Func<
    CreateAndFake.RunnerTool.RunnerOptions,
    CreateAndFake.RunnerTool.RunnerOptions
>;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RunnerTool;

public static class RunnerOptionsTests
{
    [Fact]
    internal static Task RunnerOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<RunnerOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task RunnerOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<RunnerOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void RunnerOptions_ModFormRandomizable()
    {
        typeof(RunnerMod).CreateRandomInstance().Assert().IsNot(null);
    }
}
