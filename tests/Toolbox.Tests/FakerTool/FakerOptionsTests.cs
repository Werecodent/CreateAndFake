global using FakerMod = System.Func<
    Werecodent.CreateAndFake.FakerTool.FakerOptions,
    Werecodent.CreateAndFake.FakerTool.FakerOptions
>;
using Werecodent.CreateAndFake.FakerTool;

namespace Werecodent.CreateAndFake.Tests.FakerTool;

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
        typeof(FakerMod).Tools().CreateRandomInstance().Assert().IsNotNull();
    }
}
