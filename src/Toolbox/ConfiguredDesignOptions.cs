using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.Properties;

namespace Werecodent.CreateAndFake;

/// <inheritdoc cref="DesignOptions"/>
public sealed record ConfiguredDesignOptions : DesignOptions
{
    /// <summary>Identifies the configuration section containing these settings.</summary>
    internal static string ConfigSectionName { get; } = "Design";

    /// <summary>Derives options from <see langword="this"/> overridden by <paramref name="config"/>'s values.</summary>
    /// <param name="config">Configuration with the potential overrides to use.</param>
    /// <returns>The created options.</returns>
    internal ConfiguredDesignOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(ConfigSectionName);
        if (section == null)
        {
            return this;
        }

        return this with
        {
            Seed = Config.GetValue(section, Seed),
            IterationLimit = Config.GetValue(section, IterationLimit),
            IncludeInfinityAndNaNGeneration = Config.GetValue(
                section,
                IncludeInfinityAndNaNGeneration
            ),
        };
    }
}
