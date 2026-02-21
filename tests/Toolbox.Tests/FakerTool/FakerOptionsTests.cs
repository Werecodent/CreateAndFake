global using FakerMod = System.Func<
    CreateAndFake.FakerTool.FakerOptions,
    CreateAndFake.FakerTool.FakerOptions
>;
using CreateAndFake.FakerTool;

namespace CreateAndFake.Tests.FakerTool;

public static class FakerOptionsTests
{
    [Fact]
    internal static Task FakerOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakerOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task FakerOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakerOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void FakerOptions_ModFormRandomizable()
    {
        typeof(FakerMod).CreateRandomInstance().Assert().IsNot(null);
    }
}
