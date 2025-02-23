using CreateAndFake.TesterTool;

namespace CreateAndFake.Tests.TesterTool;

public static class GenericFixerTests
{
    [Fact]
    internal static void GenericFixer_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(typeof(GenericFixer));
    }
}
