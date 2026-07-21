using CreateAndFake.Samples.BasicData;

namespace CreateAndFake.Samples.Tests.BasicData;

public static class NestedDtoTests
{
    [Fact]
    public static Task NestedDto_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<NestedDto>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task NestedDto_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<NestedDto>(
            TestContext.Current.CancellationToken
        );
    }
}
