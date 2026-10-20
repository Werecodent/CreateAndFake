using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.Design.Tests.Tooling;

public static class DesignOptionsTests
{
    [Fact]
    internal static Task DesignOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<DesignOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task DesignOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<DesignOptions>(
            TestContext.Current.CancellationToken
        );
    }
}
