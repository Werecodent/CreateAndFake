global using TesterMod = System.Func<
    CreateAndFake.Toolbox.TesterTool.TesterOptions,
    CreateAndFake.Toolbox.TesterTool.TesterOptions>;

using CreateAndFake.Toolbox.TesterTool;

namespace CreateAndFakeTests.Toolbox.TesterTool;

public static class TesterOptionsTests
{
    [Fact]
    internal static void TesterOptions_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<TesterOptions>();
    }

    [Fact]
    internal static void TesterOptions_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<TesterOptions>();
    }

    [Fact]
    internal static void TesterOptions_ModFormRandomizable()
    {
        typeof(TesterMod).CreateRandomInstance().Assert().IsNot(null);
    }
}