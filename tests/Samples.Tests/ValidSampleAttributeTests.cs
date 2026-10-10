namespace Werecodent.CreateAndFake.Samples.Tests;

public static class ValidSampleAttributeTests
{
    [Fact]
    public static Task ValidSampleAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ValidSampleAttribute>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ValidSampleAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ValidSampleAttribute>(
            TestContext.Current.CancellationToken
        );
    }
}
