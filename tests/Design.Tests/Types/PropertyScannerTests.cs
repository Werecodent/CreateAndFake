using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Tests.Types;

public static class PropertyScannerTests
{
    [Fact]
    internal static Task PropertyScanner_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<PropertyScanner>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task PropertyScanner_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<PropertyScanner>(
            TestContext.Current.CancellationToken
        );
    }
}
