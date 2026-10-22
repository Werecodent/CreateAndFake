using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class ParentLoopSampleTests
{
    [Fact]
    public static Task ParentLoopSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ParentLoopSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ParentLoopSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ParentLoopSample>(
            TestContext.Current.CancellationToken
        );
    }
}
