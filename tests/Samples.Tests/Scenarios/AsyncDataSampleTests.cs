using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class AsyncDataSampleTests
{
    [Fact]
    public static Task AsyncDataSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<AsyncDataSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task AsyncDataSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<AsyncDataSample>(
            TestContext.Current.CancellationToken
        );
    }
}
