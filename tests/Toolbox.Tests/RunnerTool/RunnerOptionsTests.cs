global using RunnerMod = System.Func<
    Werecodent.CreateAndFake.RunnerTool.RunnerOptions,
    Werecodent.CreateAndFake.RunnerTool.RunnerOptions
>;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.RunnerTool;

public static class RunnerOptionsTests
{
    [Fact]
    internal static Task RunnerOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<RunnerOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task RunnerOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<RunnerOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void RunnerOptions_ModFormRandomizable()
    {
        typeof(RunnerMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
