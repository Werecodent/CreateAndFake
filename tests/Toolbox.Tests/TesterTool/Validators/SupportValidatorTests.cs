using CreateAndFake.TesterTool.Validators;

namespace CreateAndFake.Tests.TesterTool.Validators;

public static class SupportValidatorTests
{
    [Fact]
    internal static Task SupportValidator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<SupportValidator>(
            TestContext.Current.CancellationToken
        );
    }
}
