namespace CreateAndFake.Tests.Attributes;

public static class StubAttributeTests
{
    [Fact]
    internal static Task StubAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<StubAttribute>();
    }

    [Fact]
    internal static Task StubAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<StubAttribute>();
    }
}
