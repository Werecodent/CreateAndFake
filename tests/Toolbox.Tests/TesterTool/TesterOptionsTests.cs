global using TesterMod = System.Func<
    Werecodent.CreateAndFake.TesterTool.TesterOptions,
    Werecodent.CreateAndFake.TesterTool.TesterOptions
>;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Tests.TesterTool;

public sealed class TesterOptionsTests : AutoTester<TesterOptions>
{
    protected override bool EqualsOverrideUsesFullValueEquality { get; } = false;

    [Fact]
    internal static void TesterOptions_ModFormRandomizable()
    {
        typeof(TesterMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
