using CreateAndFake.ValuerTool;

namespace CreateAndFake.Tests.ValuerTool;

public static class ValuerChainerTests
{
    [Fact]
    internal static Task ValuerChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ValuerChainer>();
    }

    [Fact]
    internal static Task ValuerChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ValuerChainer>();
    }
}
