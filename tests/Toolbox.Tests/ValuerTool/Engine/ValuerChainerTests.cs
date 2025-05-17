using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

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
