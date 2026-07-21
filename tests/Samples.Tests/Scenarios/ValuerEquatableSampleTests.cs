using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Samples.Tests.Scenarios;

public static class ValuerEquatableSampleTests
{
    [Fact]
    public static Task ValuerEquatableSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ValuerEquatableSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ValuerEquatableSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ValuerEquatableSample>(
            TestContext.Current.CancellationToken
        );
    }
}
