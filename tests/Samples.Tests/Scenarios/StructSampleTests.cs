using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Samples.Tests.Scenarios;

public static class StructSampleTests
{
    [Fact]
    public static Task StructSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<StructSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task StructSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<StructSample>(
            TestContext.Current.CancellationToken
        );
    }
}
