using System.Collections;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.DuplicatorTool;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.MutatorTool;
using Werecodent.CreateAndFake.RandomizerTool;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.TesterTool;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.Configuration.Tests;

public static class ToolSetTests
{
    private static readonly ToolSet _PlainTools = ToolSet.CreateViaSeed(Tools.Gen.Next<int>());

    private static readonly IConfigurationSection _Config = new ConfigurationBuilder()
        .AddJsonFile("testsettings.json", true)
        .AddJsonFile("testsettings.Production.json", true)
        .Build()
        .GetSection("CreateAndFake");

    [Fact]
    internal static void CreateViaConfig_SetsSeed()
    {
        _PlainTools.Asserter.Is(2, Tools.Gen.InitialSeed);
    }

    [Fact]
    internal static void CreateViaConfig_SetsAsserterOptions()
    {
        TestConfigurableOptions(Tools.Asserter.Options, _PlainTools.Asserter.Options);
        TestConfigSize<AsserterOptions>(nameof(Asserter));
    }

    [Fact]
    internal static void CreateViaConfig_SetsDuplicatorOptions()
    {
        TestConfigurableOptions(Tools.Duplicator.Options, _PlainTools.Duplicator.Options);
        TestConfigSize<DuplicatorOptions>(nameof(Duplicator));
    }

    [Fact]
    internal static void CreateViaConfig_SetsExtractorOptions()
    {
        TestConfigurableOptions(Tools.Extractor.Options, _PlainTools.Extractor.Options);
        TestConfigSize<ExtractorOptions>(nameof(Extractor));
    }

    [Fact]
    internal static void CreateViaConfig_SetsFakerOptions()
    {
        TestConfigurableOptions(Tools.Faker.Options, _PlainTools.Faker.Options);
        TestConfigSize<FakerOptions>(nameof(Faker));
    }

    [Fact]
    internal static void CreateViaConfig_SetsMutatorOptions()
    {
        TestConfigurableOptions(Tools.Mutator.Options, _PlainTools.Mutator.Options);
        TestConfigSize<MutatorOptions>(nameof(Mutator));
    }

    [Fact]
    internal static void CreateViaConfig_SetsRandomizerOptions()
    {
        TestConfigurableOptions(Tools.Randomizer.Options, _PlainTools.Randomizer.Options);
        TestConfigSize<RandomizerOptions>(nameof(Randomizer));
    }

    [Fact]
    internal static void CreateViaConfig_SetsRunnerOptions()
    {
        TestConfigurableOptions(Tools.Runner.Options, _PlainTools.Runner.Options);
        TestConfigSize<RunnerOptions>(nameof(Runner));
    }

    [Fact]
    internal static void CreateViaConfig_SetsTesterOptions()
    {
        TestConfigurableOptions(Tools.Tester.Options, _PlainTools.Tester.Options);
        TestConfigSize<TesterOptions>(nameof(Tester));
    }

    [Fact]
    internal static void CreateViaConfig_SetsValuerOptions()
    {
        TestConfigurableOptions(Tools.Valuer.Options, _PlainTools.Valuer.Options);
        TestConfigSize<ValuerOptions>(nameof(Valuer));
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

    private static void TestConfigSize<T>(string sectionName)
    {
        _PlainTools.Asserter.IsEmpty(
            _Config
                .GetSection(sectionName)
                .GetChildren()
                .Select(c => c.Key)
                .Except(
                    TypeDescriber
                        .For<T>()
                        .Properties.OnlyPublic.Where(p =>
                            Attribute.IsDefined(p, typeof(ConfigurableOptionAttribute))
                        )
                        .Select(p => p.Name)
                ),
            "Configuration had extra settings."
        );
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
