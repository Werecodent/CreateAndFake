global using ExtractorMod = System.Func<
    Werecodent.CreateAndFake.ExtractorTool.ExtractorOptions,
    Werecodent.CreateAndFake.ExtractorTool.ExtractorOptions
>;
using Werecodent.CreateAndFake.ExtractorTool;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool;

public sealed class ExtractorOptionsTests : AutoTester<ExtractorOptions>
{
    protected override bool EqualsOverrideUsesFullValueEquality { get; } = false;

    [Fact]
    internal static void ExtractorOptions_ModFormRandomizable()
    {
        typeof(ExtractorMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
