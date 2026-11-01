global using AsserterMod = System.Func<
    Werecodent.CreateAndFake.AsserterTool.AsserterOptions,
    Werecodent.CreateAndFake.AsserterTool.AsserterOptions
>;
using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Tests.AsserterTool;

public sealed class AsserterOptionsTests : AutoTester<AsserterOptions>
{
    protected override bool EqualsOverrideUsesFullValueEquality { get; } = false;

    [Fact]
    internal static void AsserterOptions_ModFormRandomizable()
    {
        typeof(AsserterMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
