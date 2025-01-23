using CreateAndFake.Toolbox.ValuerTool;

namespace CreateAndFakeTests.Toolbox.ValuerTool;

public static class ValuerChainerTests
{
    [Fact]
    internal static void ValuerChainer_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<ValuerChainer>();
    }

    [Fact]
    internal static void ValuerChainer_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<ValuerChainer>();
    }
}
