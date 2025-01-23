using CreateAndFake.Toolbox.ValuerTool;

namespace CreateAndFakeTests.Toolbox.ValuerTool;

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
