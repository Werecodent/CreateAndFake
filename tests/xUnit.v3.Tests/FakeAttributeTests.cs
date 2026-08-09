namespace Werecodent.CreateAndFake.xUnit.v3.Tests;

public static class FakeAttributeTests
{
    [Fact]
    internal static Task FakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FakeAttribute>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task FakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FakeAttribute>(
            TestContext.Current.CancellationToken
        );
    }
}
