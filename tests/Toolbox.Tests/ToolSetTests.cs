namespace CreateAndFake.Tests;

public static class ToolSetTests
{
    [Fact]
    internal static void ToolSet_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<ToolSet>();
    }

    [Fact]
    internal static void ToolSet_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<ToolSet>();
    }

    [Fact]
    internal static void ToolSet_Creatable()
    {
        ToolSet.CreateViaSeed(0).Assert().IsNot(null);
    }
}
