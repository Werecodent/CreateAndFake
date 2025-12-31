using CreateAndFake.Design.Tooling;

namespace CreateAndFake.Design.Tests.Tooling;

public class ConfigurableOptionAttributeTests
{
    [Fact]
    public Task ConfigurableOptionAttribute_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ConfigurableOptionAttribute>();
    }

    [Fact]
    public Task ConfigurableOptionAttribute_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<ConfigurableOptionAttribute>();
    }
}
