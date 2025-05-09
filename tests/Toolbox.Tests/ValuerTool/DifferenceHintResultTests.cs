using CreateAndFake.ValuerTool;

namespace CreateAndFake.Tests.ValuerTool;

public static class DifferenceHintResultTests
{
    [Fact]
    internal static Task DifferenceHintResult_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<DifferenceHintResult>();
    }

    [Fact]
    internal static Task DifferenceHintResult_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<DifferenceHintResult>();
    }

    [Fact]
    internal static void None_HasNoData()
    {
        DifferenceHintResult.None.HasData.Assert().Is(false);
    }
}
