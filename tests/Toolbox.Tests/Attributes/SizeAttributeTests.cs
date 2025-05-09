namespace CreateAndFake.Tests.Attributes;

public static class SizeAttributeTests
{
    [Fact]
    internal static Task SizeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<SizeAttribute>();
    }

    [Fact]
    internal static Task SizeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<SizeAttribute>();
    }
}
