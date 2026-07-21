using CreateAndFake.Samples.BasicData;

namespace CreateAndFake.Samples.Tests.BasicData;

public static class RecursiveDtoTests
{
    [Fact]
    public static Task RecursiveDto_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<RecursiveDto>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task RecursiveDto_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<RecursiveDto>(
            TestContext.Current.CancellationToken
        );
    }
}
