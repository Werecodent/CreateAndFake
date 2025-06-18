using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

public static class ValuerChainerTests
{
    [Fact]
    internal static Task IValuerChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<IValuerChainer>();
    }

    [Fact]
    internal static Task IValuerChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<IValuerChainer>();
    }
}
