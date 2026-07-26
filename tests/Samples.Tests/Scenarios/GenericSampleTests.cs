using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Samples.Tests.Scenarios;

public static class GenericSampleTests
{
    [Fact]
    public static Task GenericSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(GenericSample<>),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task GenericSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(GenericSample<>),
            TestContext.Current.CancellationToken
        );
    }
}
