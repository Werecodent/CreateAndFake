using System.Collections.Immutable;
using CreateAndFake.Properties;
using Microsoft.Extensions.Configuration;

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

    [Fact]
    internal static Task Config_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(Config),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(InvalidOperationException)] }
        );
    }

    [Theory, RandomData]
    internal static void GetArray_NullReturnsProperty(
        [Stub] IConfigurationSection section,
        ImmutableArray<string> data
    )
    {
        Config.GetArray(section, data).Assert().Is(data);
    }
}
