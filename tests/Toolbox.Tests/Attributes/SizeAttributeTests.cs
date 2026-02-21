namespace CreateAndFake.Tests.Attributes;

public static class SizeAttributeTests
{
    [Fact]
    internal static Task SizeAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<SizeAttribute>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task SizeAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<SizeAttribute>(
            TestContext.Current.CancellationToken
        );
    }
}
