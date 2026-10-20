using System.Reflection;
using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.FakerTool.Engine;
using Werecodent.CreateAndFake.FakerTool.Proxy;
using Werecodent.CreateAndFake.Properties;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.FakerTool;

/// <summary>Settings for manipulating <see cref="IFaker"/> behavior.</summary>
public sealed record FakerOptions : ToolHintOptions<FakerOptions, IFakeHint>
{
    /// <summary>Identifies the configuration section containing these settings.</summary>
    internal static string ConfigSectionName { get; } = nameof(Faker);

    /// <inheritdoc cref="IValuer"/>
    public required IValuer Valuer { get; init; }

    /// <summary>If set, controls the default behavior for fake calls.</summary>
    /// <value><see langword="null"/></value>
    internal Func<MethodInfo, CallData, Behavior>? FakeDefaultGenerator { get; init; } = null;

    /// <summary>Derives options from <see langword="this"/> overridden by <paramref name="config"/>'s values.</summary>
    /// <param name="config">Configuration with the potential overrides to use.</param>
    /// <returns>The created options.</returns>
    internal FakerOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(ConfigSectionName);
        if (section == null)
        {
            return this;
        }

        return this with
        {
            IncludeFrameworkHints = Config.GetValue(section, IncludeFrameworkHints),
            IncludeFoundHints = Config.GetValue(section, IncludeFoundHints),
            MaxRecursionDepth = Config.GetValue(section, MaxRecursionDepth),
        };
    }
}
