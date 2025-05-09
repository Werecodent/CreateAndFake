using CreateAndFake.TesterTool;

namespace CreateAndFake.Tests.TesterTool;

public static class GenericFixerTests
{
    [Fact]
    internal static Task GenericFixer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(typeof(GenericFixer));
    }
}
