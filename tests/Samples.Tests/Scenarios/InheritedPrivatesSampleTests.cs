using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class InheritedPrivatesSampleTests
{
    [Fact]
    public static Task InheritedPrivatesSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<InheritedPrivatesSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task InheritedPrivatesSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<InheritedPrivatesSample>(
            TestContext.Current.CancellationToken
        );
    }
}
