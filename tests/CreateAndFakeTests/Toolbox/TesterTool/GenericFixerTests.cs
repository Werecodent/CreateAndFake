using CreateAndFake.Design.Randomization;
using CreateAndFake.Toolbox.TesterTool;

namespace CreateAndFakeTests.Toolbox.TesterTool;

public static class GenericFixerTests
{
    [Fact]
    internal static void GenericFixer_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(typeof(GenericFixer));
    }

    [Fact]
    internal static void GenericFixer_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation(typeof(GenericFixer), opt => opt with { Gen = new FastRandom() });
    }
}
