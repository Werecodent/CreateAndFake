using CreateAndFake.Toolbox.AsserterTool.Fluent;

namespace CreateAndFakeTests.Toolbox.AsserterTool.Fluent;

public static class AssertComparableTests
{
    [Fact]
    internal static void AssertComparable_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<AssertComparable>();
    }

    [Fact]
    internal static void AssertComparable_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<AssertComparable>();
    }
}

