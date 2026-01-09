namespace CreateAndFake.Tests;

public static class ToolSetTests
{
    [Fact]
    internal static Task ToolSet_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ToolSet>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ToolSet_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ToolSet>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void ToolSet_Creatable()
    {
        ToolSet.CreateViaSeed(0).Assert().IsNot(null);
    }
}
