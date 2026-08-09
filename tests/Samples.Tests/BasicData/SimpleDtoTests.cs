using Werecodent.CreateAndFake.Samples.BasicData;

namespace Werecodent.CreateAndFake.Samples.Tests.BasicData;

public static class SimpleDtoTests
{
    [Fact]
    public static Task SimpleDto_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<SimpleDto>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task SimpleDto_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<SimpleDto>(
            TestContext.Current.CancellationToken
        );
    }
}
