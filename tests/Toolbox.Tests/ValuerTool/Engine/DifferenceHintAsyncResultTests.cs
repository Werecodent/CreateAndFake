using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

public static class DifferenceHintAsyncResultTests
{
    [Fact]
    internal static Task DifferenceHintAsyncResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<DifferenceHintAsyncResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task DifferenceHintAsyncResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<DifferenceHintAsyncResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void None_HasNoData()
    {
        DifferenceHintAsyncResult.None.HasData.Assert().Is(false);
    }
}
