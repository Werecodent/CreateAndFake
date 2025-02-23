global using TesterMod = System.Func<
    CreateAndFake.TesterTool.TesterOptions,
    CreateAndFake.TesterTool.TesterOptions>;

using CreateAndFake.TesterTool;

namespace CreateAndFake.Tests.TesterTool;

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