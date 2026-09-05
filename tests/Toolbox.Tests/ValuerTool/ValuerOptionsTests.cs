global using ValuerMod = System.Func<
    Werecodent.CreateAndFake.ValuerTool.ValuerOptions,
    Werecodent.CreateAndFake.ValuerTool.ValuerOptions
>;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Tests.ValuerTool;

public static class ValuerOptionsTests
{
    [Fact]
    internal static Task ValuerOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ValuerOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ValuerOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ValuerOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void ValuerOptions_ModFormRandomizable()
    {
        typeof(ValuerMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
