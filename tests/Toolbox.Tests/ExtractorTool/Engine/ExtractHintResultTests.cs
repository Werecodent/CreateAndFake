using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.Tests.ExtractorTool.Engine;

public static class ExtractHintResultTests
{
    [Fact]
    internal static Task ExtractHintResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ExtractHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ExtractHintResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ExtractHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void None_HasNoData()
    {
        ExtractHintResult.None.HasData.Assert().Is(false);
    }
}
