using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.Tests.ValuerTool.Engine;

public static class HashCodeHintResultTests
{
    [Fact]
    internal static Task HashCodeHintResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<HashCodeHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task HashCodeHintResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<HashCodeHintResult>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void None_HasNoData()
    {
        HashCodeHintResult.None.HasData.Assert().Is(false);
    }
}
