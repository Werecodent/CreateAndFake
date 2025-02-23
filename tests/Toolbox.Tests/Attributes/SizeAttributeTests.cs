namespace CreateAndFake.Tests.Attributes;

public static class SizeAttributeTests
{
    [Fact]
    internal static void SizeAttribute_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<SizeAttribute>();
    }

    [Fact]
    internal static void SizeAttribute_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<SizeAttribute>();
    }
}
