global using ExtractorMod = System.Func<
    CreateAndFake.Toolbox.ExtractorTool.ExtractorOptions,
    CreateAndFake.Toolbox.ExtractorTool.ExtractorOptions>;

using CreateAndFake.Toolbox.ExtractorTool;

namespace CreateAndFakeTests.Toolbox.ExtractorTool;

public static class ExtractorOptionsTests
{
    [Fact]
    internal static void ExtractorOptions_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<ExtractorOptions>();
    }

    [Fact]
    internal static void ExtractorOptions_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<ExtractorOptions>();
    }

    [Fact]
    internal static void ExtractorOptions_ModFormRandomizable()
    {
        typeof(ExtractorMod).CreateRandomInstance().Assert().IsNot(null);
    }
}