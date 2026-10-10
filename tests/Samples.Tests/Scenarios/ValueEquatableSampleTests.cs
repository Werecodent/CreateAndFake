using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Samples.Tests.Scenarios;

public static class ValueEquatableSampleTests
{
    [Fact]
    public static Task ValueEquatableSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ValueEquatableSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ValueEquatableSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ValueEquatableSample>(
            TestContext.Current.CancellationToken
        );
    }
}
