global using ValuerMod = System.Func<
    Werecodent.CreateAndFake.ValuerTool.ValuerOptions,
    Werecodent.CreateAndFake.ValuerTool.ValuerOptions
>;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Tests.ValuerTool;

public sealed class ValuerOptionsTests : AutoTester<ValuerOptions>
{
    protected override bool EqualsOverrideUsesFullValueEquality { get; } = false;

    [Fact]
    internal static void ValuerOptions_ModFormRandomizable()
    {
        typeof(ValuerMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
