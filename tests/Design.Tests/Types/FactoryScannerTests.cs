using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tests.Types;

public static class FactoryScannerTests
{
    [Fact]
    internal static Task FactoryScanner_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<FactoryScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task FactoryScanner_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<FactoryScanner>(
            TestContext.Current.CancellationToken
        );
    }
}
