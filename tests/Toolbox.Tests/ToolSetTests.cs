namespace CreateAndFake.Tests;

public static class ToolSetTests
{
    [Fact]
    internal static Task ToolSet_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ToolSet>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ToolSet_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ToolSet>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void ToolSet_Creatable()
    {
        ToolSet.CreateViaSeed(0).Assert().IsNotNull();
    }
}
