using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class StatelessSampleTests
{
    [Fact]
    public static Task StatelessSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<StatelessSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task StatelessSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<StatelessSample>(
            TestContext.Current.CancellationToken
        );
    }
}
