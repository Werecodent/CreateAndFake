using CreateAndFake.DuplicatorTool;

namespace CreateAndFake.Tests.DuplicatorTool;

public static class DuplicatorChainerTests
{
    [Fact]
    internal static Task DuplicatorChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<DuplicatorChainer>();
    }

    [Fact]
    internal static Task DuplicatorChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<DuplicatorChainer>();
    }
}
