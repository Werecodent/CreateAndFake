namespace Werecodent.CreateAndFake.Tests;

public static class ConfiguredDesignOptionsTests
{
    [Fact]
    internal static Task ConfiguredDesignOptions_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<ConfiguredDesignOptions>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task ConfiguredDesignOptions_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<ConfiguredDesignOptions>(
            TestContext.Current.CancellationToken
        );
    }
}
