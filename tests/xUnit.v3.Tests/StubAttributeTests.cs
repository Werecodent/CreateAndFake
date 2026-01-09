namespace CreateAndFake.xUnit.v3.Tests;

public static class StubAttributeTests
{
    [Fact]
    internal static Task StubAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<StubAttribute>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task StubAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<StubAttribute>(
            TestContext.Current.CancellationToken
        );
    }
}
