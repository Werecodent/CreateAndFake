using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Engine;

public static class ExtractHintResultTests
{
    [Fact]
    internal static Task ExtractHintResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ExtractHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ExtractHintResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ExtractHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void None_HasNoData()
    {
        ExtractHintResult.None.HasData.Assert().Is(false);
    }
}
