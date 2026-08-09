using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Tests.TesterTool;

public static class GenericFixerTests
{
    [Fact]
    internal static Task GenericFixer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(GenericFixer),
            TestContext.Current.CancellationToken
        );
    }
}
