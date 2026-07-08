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
