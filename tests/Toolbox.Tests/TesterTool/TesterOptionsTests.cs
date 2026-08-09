global using TesterMod = System.Func<
    Werecodent.CreateAndFake.TesterTool.TesterOptions,
    Werecodent.CreateAndFake.TesterTool.TesterOptions
>;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Tests.TesterTool;

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
        typeof(TesterMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
