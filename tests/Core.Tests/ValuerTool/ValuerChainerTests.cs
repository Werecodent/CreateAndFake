using CreateAndFake.ValuerTool;

namespace CreateAndFake.Tests.ValuerTool;

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
