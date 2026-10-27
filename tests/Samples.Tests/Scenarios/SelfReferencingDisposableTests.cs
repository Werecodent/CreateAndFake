using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class SelfReferencingDisposableTests
{
    [Fact]
    public static Task SelfReferencingDisposable_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<SelfReferencingDisposable>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task SelfReferencingDisposable_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<SelfReferencingDisposable>(
            TestContext.Current.CancellationToken
        );
    }
}
