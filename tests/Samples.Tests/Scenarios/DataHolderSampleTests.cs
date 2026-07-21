using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Samples.Tests.Scenarios;

public static class DataHolderSampleTests
{
    [Fact]
    public static Task DataHolderSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<DataHolderSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task DataHolderSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<DataHolderSample>(
            TestContext.Current.CancellationToken
        );
    }
}
