global using ValuerMod = System.Func<
    CreateAndFake.ValuerTool.ValuerOptions,
    CreateAndFake.ValuerTool.ValuerOptions
>;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.Tests.ValuerTool;

public static class ValuerOptionsTests
{
    [Fact]
    internal static Task ValuerOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ValuerOptions>();
    }

    [Fact]
    internal static Task ValuerOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ValuerOptions>();
    }

    [Fact]
    internal static void ValuerOptions_ModFormRandomizable()
    {
        typeof(ValuerMod).CreateRandomInstance().Assert().IsNot(null);
    }
}
