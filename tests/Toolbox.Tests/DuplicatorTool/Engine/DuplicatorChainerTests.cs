using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.Tests.DuplicatorTool.Engine;

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
