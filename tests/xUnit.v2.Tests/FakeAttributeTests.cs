namespace Werecodent.CreateAndFake.xUnit.v2.Tests;

public static class FakeAttributeTests
{
    [Fact]
    internal static Task FakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakeAttribute>(CancellationToken.None);
    }

    [Fact]
    internal static Task FakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakeAttribute>(CancellationToken.None);
    }
}
