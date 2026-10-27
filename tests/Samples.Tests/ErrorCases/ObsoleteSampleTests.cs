using Werecodent.CreateAndFake.Samples.ErrorCases;

namespace Werecodent.CreateAndFake.Samples.Tests.ErrorCases;

public static class ObsoleteSampleTests
{
    [Fact]
    public static Task ObsoleteSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ObsoleteSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ObsoleteSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ObsoleteSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ObsoleteSample_NoParameterMutationCanIncludeObsolete()
    {
        return Tools
            .Tester.PreventsParameterMutationAsync<ObsoleteSample>(
                TestContext.Current.CancellationToken,
                opt => opt with { IgnoreObsoleteMethods = false }
            )
            .Assert()
            .ThrowsAsync<InvalidOperationException>(TestContext.Current.CancellationToken);
    }
}
