using CreateAndFake.Toolbox.DuplicatorTool;

namespace CreateAndFakeTests.Toolbox.DuplicatorTool;

public static class DuplicatorChainerTests
{
    [Fact]
    internal static void DuplicatorChainer_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<DuplicatorChainer>();
    }

    [Fact]
    internal static void DuplicatorChainer_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<DuplicatorChainer>();
    }
}
