namespace CreateAndFake.xUnit.v2.Tests;

public static class SizeAttributeTests
{
    [Fact]
    internal static Task SizeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<FakeAttribute>();
    }

    [Fact]
    internal static Task SizeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<FakeAttribute>();
    }
}
