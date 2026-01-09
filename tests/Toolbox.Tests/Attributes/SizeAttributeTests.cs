namespace CreateAndFake.Tests.Attributes;

public static class SizeAttributeTests
{
    [Fact]
    internal static Task SizeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<SizeAttribute>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task SizeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<SizeAttribute>(
            TestContext.Current.CancellationToken
        );
    }
}
