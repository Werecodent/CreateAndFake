using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class DoubleEnumerableSampleTests
{
    [Fact]
    public static Task DoubleEnumerableSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<DoubleEnumerableSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task DoubleEnumerableSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<DoubleEnumerableSample>(
            TestContext.Current.CancellationToken
        );
    }
}
