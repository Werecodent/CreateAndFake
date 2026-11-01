global using FakerMod = System.Func<
    Werecodent.CreateAndFake.FakerTool.FakerOptions,
    Werecodent.CreateAndFake.FakerTool.FakerOptions
>;
using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.Tests.FakerTool;

public sealed class FakerOptionsTests : AutoTester<FakerOptions>
{
    protected override bool EqualsOverrideUsesFullValueEquality { get; } = false;

    [Fact]
    internal static void FakerOptions_ModFormRandomizable()
    {
        typeof(FakerMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
