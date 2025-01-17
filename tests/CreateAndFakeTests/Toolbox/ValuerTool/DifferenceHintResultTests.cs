using CreateAndFake.Toolbox.ValuerTool;

namespace CreateAndFakeTests.Toolbox.ValuerTool;

public static class DifferenceHintResultTests
{
    [Fact]
    internal static void DifferenceHintResult_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<DifferenceHintResult>();
    }

    [Fact]
    internal static void DifferenceHintResult_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<DifferenceHintResult>();
    }

    [Fact]
    internal static void None_HasNoData()
    {
        DifferenceHintResult.None.HasData.Assert().Is(false);
    }
}