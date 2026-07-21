using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Samples.Tests.Scenarios;

public static class FieldSampleTests
{
    [Fact]
    public static Task FieldSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FieldSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task FieldSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FieldSample>(
            TestContext.Current.CancellationToken
        );
    }
}
