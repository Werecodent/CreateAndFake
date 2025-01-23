namespace CreateAndFakeTests.Attributes;

public static class StubAttributeTests
{
    [Fact]
    internal static void StubAttribute_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<StubAttribute>();
    }

    [Fact]
    internal static void StubAttribute_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<StubAttribute>();
    }
}
