namespace Werecodent.CreateAndFake.Samples.Tests;

public static class SampleGeneratorTests
{
    [Fact]
    internal static void Debug_SampleGenerator_AllValidSamples()
    {
        SampleGenerator.AllValidDataSamples.Assert().Debug();
    }

    [Fact]
    public static Task SampleGenerator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(SampleGenerator),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task SampleGenerator_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(SampleGenerator),
            TestContext.Current.CancellationToken
        );
    }
}
