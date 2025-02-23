namespace CreateAndFake.Tests.Attributes;

public static class FakeAttributeTests
{
    [Fact]
    internal static void FakeAttribute_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<FakeAttribute>();
    }

    [Fact]
    internal static void FakeAttribute_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<FakeAttribute>();
    }
}
