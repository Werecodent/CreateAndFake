using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class PrivateValuerEquatableSampleTests
{
    [Fact]
    public static Task PrivateValuerEquatableSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<PrivateValuerEquatableSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task PrivateValuerEquatableSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<PrivateValuerEquatableSample>(
            TestContext.Current.CancellationToken
        );
    }
}
