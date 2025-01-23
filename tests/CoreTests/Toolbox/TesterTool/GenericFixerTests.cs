using CreateAndFake.Toolbox.TesterTool;

namespace CreateAndFakeTests.Toolbox.TesterTool;

public static class GenericFixerTests
{
    [Fact]
    internal static void GenericFixer_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(typeof(GenericFixer));
    }
}
