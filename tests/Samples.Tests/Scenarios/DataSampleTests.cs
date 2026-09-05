using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class DataSampleTests
{
    [Fact]
    public static Task DataSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<DataSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task DataSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<DataSample>(
            TestContext.Current.CancellationToken
        );
    }
}
