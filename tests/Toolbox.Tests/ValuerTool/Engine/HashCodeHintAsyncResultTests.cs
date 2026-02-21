using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

public static class HashCodeHintAsyncResultTests
{
    [Fact]
    internal static Task HashCodeHintAsyncResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<HashCodeHintAsyncResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task HashCodeHintAsyncResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<HashCodeHintAsyncResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void None_HasNoData()
    {
        HashCodeHintAsyncResult.None.HasData.Assert().Is(false);
    }
}
