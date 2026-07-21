using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Samples.Tests.Scenarios;

public static class ValuerComparableSampleTests
{
    [Fact]
    public static Task ValuerComparableSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ValuerComparableSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ValuerComparableSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ValuerComparableSample>(
            TestContext.Current.CancellationToken
        );
    }
}
