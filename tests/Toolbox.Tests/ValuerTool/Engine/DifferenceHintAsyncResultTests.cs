using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Engine;

public static class DifferenceHintAsyncResultTests
{
    [Fact]
    internal static Task DifferenceHintAsyncResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<DifferenceHintAsyncResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task DifferenceHintAsyncResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<DifferenceHintAsyncResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void None_HasNoData()
    {
        DifferenceHintAsyncResult.None.HasData.Assert().Is(false);
    }
}
