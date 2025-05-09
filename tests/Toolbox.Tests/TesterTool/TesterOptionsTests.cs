global using TesterMod = System.Func<
    CreateAndFake.TesterTool.TesterOptions,
    CreateAndFake.TesterTool.TesterOptions
>;
using CreateAndFake.TesterTool;

namespace CreateAndFake.Tests.TesterTool;

public static class TesterOptionsTests
{
    [Fact]
    internal static Task TesterOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<TesterOptions>();
    }

    [Fact]
    internal static Task TesterOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<TesterOptions>();
    }

    [Fact]
    internal static void TesterOptions_ModFormRandomizable()
    {
        typeof(TesterMod).CreateRandomInstance().Assert().IsNot(null);
    }
}
