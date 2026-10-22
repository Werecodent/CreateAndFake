namespace Werecodent.CreateAndFake.Samples.Tests;

public static class InvalidSampleAttributeTests
{
    [Fact]
    public static Task InvalidSampleAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<InvalidSampleAttribute>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task InvalidSampleAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<InvalidSampleAttribute>(
            TestContext.Current.CancellationToken
        );
    }
}
