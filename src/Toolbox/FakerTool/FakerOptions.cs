using System.Reflection;
using CreateAndFake.Design.Tooling;
using CreateAndFake.FakerTool.Engine;
using CreateAndFake.Properties;
using CreateAndFake.ValuerTool;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.FakerTool;

/// <summary>Configuration for controlling faking behavior.</summary>
public sealed record FakerOptions : ToolHintOptions<FakerOptions, IFakeHint>
{
    /// <summary>Handles comparisons.</summary>
    public required IValuer? Valuer { get; init; }

    /// <summary>If set, controls the default result returned from fake calls.</summary>
    public Func<MethodInfo, object?>? FakeDefaultGenerator { get; init; }

    /// <summary>
    ///     Creates options from <see langword="this"/>
    ///     overridden with values from <paramref name="config"/>.
    /// </summary>
    /// <param name="config">Configuration with overrides to use.</param>
    /// <returns>The created options.</returns>
    internal FakerOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(nameof(Faker));
        if (section == null)
        {
            return this;
        }

        return this with
        {
            IncludeFrameworkHints = Config.GetValue(section, IncludeFrameworkHints),
            IncludeFoundHints = Config.GetValue(section, IncludeFoundHints),
            MaxHintRecursion = Config.GetValue(section, MaxHintRecursion),
        };
    }
}
