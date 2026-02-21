using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

public static class DifferenceHintResultTests
{
    [Fact]
    internal static Task DifferenceHintResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<DifferenceHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task DifferenceHintResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<DifferenceHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void None_HasNoData()
    {
        DifferenceHintResult.None.HasData.Assert().Is(false);
    }
}
