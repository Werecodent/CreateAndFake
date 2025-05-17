using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

public static class HashCodeHintAsyncResultTests
{
    [Fact]
    internal static Task HashCodeHintAsyncResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<HashCodeHintAsyncResult>();
    }

    [Fact]
    internal static Task HashCodeHintAsyncResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<HashCodeHintAsyncResult>();
    }

    [Fact]
    internal static void None_HasNoData()
    {
        HashCodeHintAsyncResult.None.HasData.Assert().Is(false);
    }
}
