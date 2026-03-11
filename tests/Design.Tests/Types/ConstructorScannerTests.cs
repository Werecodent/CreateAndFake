using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tests.Types;

public static class ConstructorScannerTests
{
    [Fact]
    internal static Task ConstructorScanner_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ConstructorScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ConstructorScanner_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ConstructorScanner>(
            TestContext.Current.CancellationToken
        );
    }
}
