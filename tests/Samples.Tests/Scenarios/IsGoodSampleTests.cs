using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class IsGoodSampleTests
{
    [Fact]
    public static Task IsGoodSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<IsGoodSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task IsGoodSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<IsGoodSample>(
            TestContext.Current.CancellationToken
        );
    }
}
