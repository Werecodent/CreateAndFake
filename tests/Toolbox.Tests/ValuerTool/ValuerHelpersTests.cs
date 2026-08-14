using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Tests.ValuerTool;

public static class ValuerHelpersTests
{
    [Fact]
    internal static Task ValuerHelpers_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(ValuerHelpers),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ValuerHelpers_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(ValuerHelpers),
            TestContext.Current.CancellationToken
        );
    }
}
