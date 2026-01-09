namespace CreateAndFake.xUnit.v3.Tests;

public static class SizeAttributeTests
{
    [Fact]
    internal static Task SizeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<FakeAttribute>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task SizeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<FakeAttribute>(
            TestContext.Current.CancellationToken
        );
    }
}
