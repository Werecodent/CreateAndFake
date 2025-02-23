using CreateAndFake.ValuerTool;

namespace CreateAndFake.Tests.ValuerTool;

public static class DifferenceTests
{
    [Fact]
    internal static void Difference_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<Difference>();
    }

    [Fact]
    internal static void Difference_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<Difference>();
    }
}
