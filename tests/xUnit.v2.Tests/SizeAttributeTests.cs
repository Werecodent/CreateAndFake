namespace Werecodent.CreateAndFake.xUnit.v2.Tests;

public static class SizeAttributeTests
{
    [Fact]
    internal static Task SizeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakeAttribute>(CancellationToken.None);
    }

    [Fact]
    internal static Task SizeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakeAttribute>(CancellationToken.None);
    }
}
