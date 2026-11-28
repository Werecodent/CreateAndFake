global using RunnerMod = System.Func<
    Werecodent.CreateAndFake.RunnerTool.RunnerOptions,
    Werecodent.CreateAndFake.RunnerTool.RunnerOptions
>;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.RunnerTool;

public sealed class RunnerOptionsTests : AutoTester<RunnerOptions>
{
    protected override bool EqualsOverrideUsesFullValueEquality { get; } = false;

    [Fact]
    internal static void RunnerOptions_ModFormRandomizable()
    {
        typeof(RunnerMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
