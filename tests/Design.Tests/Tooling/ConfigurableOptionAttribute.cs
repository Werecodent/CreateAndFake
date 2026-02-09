using CreateAndFake.Design.Tooling;

namespace CreateAndFake.Design.Tests.Tooling;

public static class ConfigurableOptionAttributeTests
{
    [Fact]
    public static Task ConfigurableOptionAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ConfigurableOptionAttribute>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    public static Task ConfigurableOptionAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ConfigurableOptionAttribute>(
            TestContext.Current.CancellationToken
        );
    }
}
