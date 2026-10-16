using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class ContentEnumerableSampleTests
{
    [Fact]
    public static Task ContentEnumerableSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ContentEnumerableSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ContentEnumerableSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ContentEnumerableSample>(
            TestContext.Current.CancellationToken
        );
    }
}
