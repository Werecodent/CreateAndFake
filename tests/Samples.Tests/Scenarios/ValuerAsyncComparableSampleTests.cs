using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Samples.Tests.Scenarios;

public static class ValuerAsyncComparableSampleTests
{
    [Fact]
    public static Task ValuerAsyncComparableSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ValuerAsyncComparableSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ValuerAsyncComparableSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ValuerAsyncComparableSample>(
            TestContext.Current.CancellationToken
        );
    }
}
