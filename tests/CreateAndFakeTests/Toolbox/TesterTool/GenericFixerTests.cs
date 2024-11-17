using CreateAndFake.Design.Randomization;
using CreateAndFake.Toolbox.TesterTool;

namespace CreateAndFakeTests.Toolbox.TesterTool;

public static class GenericFixerTests
{
    [Fact]
    internal static void GenericFixer_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<GenericFixer>();
    }

    [Theory, RandomData]
    internal static void GenericFixer_NoParameterMutation(IRandom gen)
    {
        Tools.Tester.PreventsParameterMutation(new GenericFixer(Tools.Tester.Options with { Gen = gen }));
    }
}
