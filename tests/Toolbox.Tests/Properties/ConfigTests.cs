using CreateAndFake.Properties;

namespace CreateAndFake.Tests.Properties;

public static class ConfigTests
{
    [Fact]
    internal static Task Config_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(Config),
            TestContext.Current.CancellationToken
        );
    }
}
