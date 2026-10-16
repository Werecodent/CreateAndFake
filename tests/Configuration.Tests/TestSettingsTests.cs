using System.Collections;
using System.Reflection;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.Configuration.Tests;

public static class TestSettingsTests
{
    private static readonly ToolSet _PlainTools = ToolSet.CreateViaSeed(Tools.Gen.Next<int>());

    [Fact]
    internal static Task VerifyTestSettingsConfig_Default()
    {
        return _PlainTools.Tester.ValidateTestSettingsConfigAsync(
            null,
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task VerifyTestSettingsConfig_Production()
    {
        return _PlainTools.Tester.ValidateTestSettingsConfigAsync(
            "Production",
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static void CreateViaConfig_SetsSeed()
    {
        _PlainTools.Asserter.Is(2, Tools.Gen.InitialSeed);
    }

    [Fact]
    internal static void CreateViaConfig_SetsAsserterOptions()
    {
        TestConfigurableOptions(Tools.Asserter.Options, _PlainTools.Asserter.Options);
    }

    [Fact]
    internal static void CreateViaConfig_SetsDuplicatorOptions()
    {
        TestConfigurableOptions(Tools.Duplicator.Options, _PlainTools.Duplicator.Options);
    }

    [Fact]
    internal static void CreateViaConfig_SetsExtractorOptions()
    {
        TestConfigurableOptions(Tools.Extractor.Options, _PlainTools.Extractor.Options);
    }

    [Fact]
    internal static void CreateViaConfig_SetsFakerOptions()
    {
        TestConfigurableOptions(Tools.Faker.Options, _PlainTools.Faker.Options);
    }

    [Fact]
    internal static void CreateViaConfig_SetsMutatorOptions()
    {
        TestConfigurableOptions(Tools.Mutator.Options, _PlainTools.Mutator.Options);
    }

    [Fact]
    internal static void CreateViaConfig_SetsRandomizerOptions()
    {
        TestConfigurableOptions(Tools.Randomizer.Options, _PlainTools.Randomizer.Options);
    }

    [Fact]
    internal static void CreateViaConfig_SetsRunnerOptions()
    {
        TestConfigurableOptions(Tools.Runner.Options, _PlainTools.Runner.Options);
    }

    [Fact]
    internal static void CreateViaConfig_SetsTesterOptions()
    {
        TestConfigurableOptions(Tools.Tester.Options, _PlainTools.Tester.Options);
    }

    [Fact]
    internal static void CreateViaConfig_SetsValuerOptions()
    {
        TestConfigurableOptions(Tools.Valuer.Options, _PlainTools.Valuer.Options);
    }

    private static void TestConfigurableOptions<T>(T configOptions, T plainOptions)
    {
        Dictionary<PropertyInfo, object> invalids = [];
        foreach (
            PropertyInfo prop in TypeDescriber
                .For<T>()
                .Properties.OnlyPublic.Where(p =>
                    Attribute.IsDefined(p, typeof(ConfigurableOptionAttribute))
                )
        )
        {
            object currentValue = prop.GetValue(configOptions);
            if (_PlainTools.Valuer.Equals(currentValue, prop.GetValue(plainOptions)))
            {
                invalids.Add(prop, currentValue);
            }
            else if (currentValue is IEnumerable collection)
            {
                bool hasData = false;
                foreach (object item in collection)
                {
                    hasData = true;
                }

                if (!hasData)
                {
                    invalids.Add(prop, "[]");
                }
            }
        }
        _PlainTools.Asserter.IsEmpty(invalids, "Options were not set for properties.");
    }

    [Fact]
    internal static void FindEnvironmentName_PrioritizesSetValue()
    {
        string value1 = _PlainTools.Randomizer.Create<string>();
        string value2 = _PlainTools.Randomizer.Create<string>();

        _PlainTools.Asserter.Is(ToolSet.FindEnvironmentName(), "Production");
        TestEnvironmentName("DOTNET_ENVIRONMENT", value1);
        TestEnvironmentName("ASPNETCORE_ENVIRONMENT", value2);
        _PlainTools.Asserter.Is(ToolSet.FindEnvironmentName(), "Production");
    }

    private static void TestEnvironmentName(string name, string value)
    {
        string originalValue = Environment.GetEnvironmentVariable(name);

        Environment.SetEnvironmentVariable(name, value);
        try
        {
            _PlainTools.Asserter.Is(ToolSet.FindEnvironmentName(), value);
        }
        finally
        {
            Environment.SetEnvironmentVariable(name, originalValue);
        }
    }
}
