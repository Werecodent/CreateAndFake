using CreateAndFake.ValuerTool;

namespace CreateAndFake.Tests.ValuerTool;

public static class HashCodeHintResultTests
{
    [Fact]
    internal static void HashCodeHintResult_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<HashCodeHintResult>();
    }

    [Fact]
    internal static void HashCodeHintResult_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<HashCodeHintResult>();
    }

    [Fact]
    internal static void None_HasNoData()
    {
        HashCodeHintResult.None.HasData.Assert().Is(false);
    }
}
