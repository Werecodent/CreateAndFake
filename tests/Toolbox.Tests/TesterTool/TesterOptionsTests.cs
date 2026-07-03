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
        return Tools.Tester.PreventsNullRefExceptionAsync<TesterOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task TesterOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<TesterOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void TesterOptions_ModFormRandomizable()
    {
        typeof(TesterMod).Tools().CreateRandomInstance().Assert().IsNot(null);
    }
}
