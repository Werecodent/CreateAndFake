global using AsserterMod = System.Func<
    CreateAndFake.AsserterTool.AsserterOptions,
    CreateAndFake.AsserterTool.AsserterOptions
>;
using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.AsserterTool;

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
        typeof(AsserterMod).CreateRandomInstance().Assert().IsNot(null);
    }
}
