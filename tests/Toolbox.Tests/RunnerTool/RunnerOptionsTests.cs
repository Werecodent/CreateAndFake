global using RunnerMod = System.Func<
    CreateAndFake.RunnerTool.RunnerOptions,
    CreateAndFake.RunnerTool.RunnerOptions>;

using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RunnerTool;

public static class RunnerOptionsTests
{
    [Fact]
    internal static void RunnerOptions_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<RunnerOptions>();
    }

    [Fact]
    internal static void RunnerOptions_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<RunnerOptions>();
    }

    [Fact]
    internal static void RunnerOptions_ModFormRandomizable()
    {
        typeof(RunnerMod).CreateRandomInstance().Assert().IsNot(null);
    }
}