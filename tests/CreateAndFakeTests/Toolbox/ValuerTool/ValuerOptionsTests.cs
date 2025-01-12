global using ValuerMod = System.Func<
    CreateAndFake.Toolbox.ValuerTool.ValuerOptions,
    CreateAndFake.Toolbox.ValuerTool.ValuerOptions>;

using CreateAndFake.Toolbox.ValuerTool;

namespace CreateAndFakeTests.Toolbox.ValuerTool;

public static class ValuerOptionsTests
{
    [Fact]
    internal static void ValuerOptions_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<ValuerOptions>();
    }

    [Fact]
    internal static void ValuerOptions_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<ValuerOptions>();
    }

    [Fact]
    internal static void ValuerOptions_ModFormRandomizable()
    {
        typeof(ValuerMod).CreateRandomInstance().Assert().IsNot(null);
    }
}