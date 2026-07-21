using CreateAndFake.Samples.BasicData;

namespace CreateAndFake.Samples.Tests.BasicData;

public static class CompleteDtoTests
{
    [Fact]
    public static Task CompleteDto_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<CompleteDto>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task CompleteDto_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<CompleteDto>(
            TestContext.Current.CancellationToken
        );
    }
}
