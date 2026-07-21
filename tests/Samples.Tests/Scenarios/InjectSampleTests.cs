using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Samples.Tests.Scenarios;

public static class InjectSampleTests
{
    [Fact]
    public static Task InjectSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<InjectSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task InjectSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<InjectSample>(
            TestContext.Current.CancellationToken
        );
    }
}
