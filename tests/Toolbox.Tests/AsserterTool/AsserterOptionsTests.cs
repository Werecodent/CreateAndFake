global using AsserterMod = System.Func<
    Werecodent.CreateAndFake.AsserterTool.AsserterOptions,
    Werecodent.CreateAndFake.AsserterTool.AsserterOptions
>;
using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Tests.AsserterTool;

public static class AsserterOptionsTests
{
    [Fact]
    internal static Task AsserterOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AsserterOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task AsserterOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AsserterOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void AsserterOptions_ModFormRandomizable()
    {
        typeof(AsserterMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
