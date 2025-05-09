using CreateAndFake.ValuerTool;

namespace CreateAndFake.Tests.ValuerTool;

public static class DifferenceTests
{
    [Fact]
    internal static Task Difference_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<Difference>();
    }

    [Fact]
    internal static Task Difference_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<Difference>();
    }
}
