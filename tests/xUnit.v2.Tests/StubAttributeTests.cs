namespace CreateAndFake.xUnit.v2.Tests;

public static class StubAttributeTests
{
    [Fact]
    internal static Task StubAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<StubAttribute>(CancellationToken.None);
    }

    [Fact]
    internal static Task StubAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<StubAttribute>(CancellationToken.None);
    }
}
