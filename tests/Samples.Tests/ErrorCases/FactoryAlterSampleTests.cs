using Werecodent.CreateAndFake.Samples.ErrorCases;

namespace Werecodent.CreateAndFake.Samples.Tests.ErrorCases;

public static class FactoryAlterSampleTests
{
    [Fact]
    public static Task FactoryAlterSample_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FactoryAlterSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task FactoryAlterSample_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FactoryAlterSample>(
            TestContext.Current.CancellationToken
        );
    }
}
