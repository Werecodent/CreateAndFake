using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Samples.Tests.Scenarios;

public static class ScopeSampleTests
{
    [Fact]
    public static Task ScopeSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ScopeSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ScopeSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ScopeSample>(
            TestContext.Current.CancellationToken
        );
    }
}
