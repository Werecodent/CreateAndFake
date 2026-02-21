global using ExtractorMod = System.Func<
    CreateAndFake.ExtractorTool.ExtractorOptions,
    CreateAndFake.ExtractorTool.ExtractorOptions
>;
using CreateAndFake.ExtractorTool;

namespace CreateAndFake.Tests.ExtractorTool;

public static class ExtractorOptionsTests
{
    [Fact]
    internal static Task ExtractorOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ExtractorOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ExtractorOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ExtractorOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void ExtractorOptions_ModFormRandomizable()
    {
        typeof(ExtractorMod).CreateRandomInstance().Assert().IsNot(null);
    }
}
