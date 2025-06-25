namespace CreateAndFake.xUnit.v3.Tests;

public static class FakeAttributeTests
{
    [Fact]
    internal static Task FakeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<FakeAttribute>();
    }

    [Fact]
    internal static Task FakeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<FakeAttribute>();
    }
}
