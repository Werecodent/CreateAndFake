using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class ChildWithParentSampleTests
{
    [Fact]
    public static Task ChildWithParentSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ChildWithParentSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ChildWithParentSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ChildWithParentSample>(
            TestContext.Current.CancellationToken
        );
    }
}
