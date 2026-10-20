using System.Collections.Immutable;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.DuplicatorTool;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.MutatorTool;
using Werecodent.CreateAndFake.Properties;
using Werecodent.CreateAndFake.RandomizerTool;

namespace Werecodent.CreateAndFake.RunnerTool;

/// <summary>Configuration for manipulating <see cref="IRunner"/> behavior.</summary>
public sealed record RunnerOptions : ToolOptions<RunnerOptions>
{
    /// <summary>Identifies the configuration section containing these settings.</summary>
    internal static string ConfigSectionName { get; } = nameof(Runner);

    /// <inheritdoc cref="IRandomizer"/>
    public required IRandomizer Randomizer { get; init; }

    /// <inheritdoc cref="IMutator"/>
    public required IMutator Mutator { get; init; }

    /// <inheritdoc cref="IDuplicator"/>
    public required IDuplicator Duplicator { get; init; }

    /// <inheritdoc cref="IFaker"/>
    public required IFaker Faker { get; init; }

    /// <summary>Attaches <see cref="IReflectableType"/> when faking <see cref="Type"/>s.</summary>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool InheritIReflectableTypeOnFakedType { get; init; } = false;

    /// <summary>Option for which methods to include.</summary>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool IncludeFinalize { get; init; } = false;

    /// <summary>Option for which methods to include.</summary>
    /// <value><see langword="false"/></value>
    [ConfigurableOption]
    public bool IncludeDispose { get; init; } = false;

    /// <summary>Option for which methods to include.</summary>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeStaticMethods { get; init; } = true;

    /// <summary>Option for which methods to include.</summary>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeInstanceMethods { get; init; } = true;

    /// <summary>Option for which methods to include.</summary>
    /// <value><see langword="true"/></value>
    [ConfigurableOption]
    public bool IncludeBaseObjectMethods { get; init; } = true;

    /// <summary>How long to wait for methods to complete.</summary>
    /// <value><c>00:00:50.0000000</c></value>
    [ConfigurableOption]
    public TimeSpan Timeout { get; init; } = new(0, 0, 50);

    /// <summary>Values to inject into called methods.</summary>
    /// <value><c>[]</c></value>
    public ImmutableArray<object?> InjectionValues { get; init; } = [];

    /// <summary>Derives options from <see langword="this"/> overridden by <paramref name="config"/>'s values.</summary>
    /// <param name="config">Configuration with the potential overrides to use.</param>
    /// <returns>The created options.</returns>
    internal RunnerOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(ConfigSectionName);
        if (section == null)
        {
            return this;
        }

        return this with
        {
            IncludeBaseObjectMethods = Config.GetValue(section, IncludeBaseObjectMethods),
            IncludeInstanceMethods = Config.GetValue(section, IncludeInstanceMethods),
            IncludeStaticMethods = Config.GetValue(section, IncludeStaticMethods),
            IncludeFinalize = Config.GetValue(section, IncludeFinalize),
            IncludeDispose = Config.GetValue(section, IncludeDispose),
            Timeout = Config.GetValue(section, Timeout),
            InheritIReflectableTypeOnFakedType = Config.GetValue(
                section,
                InheritIReflectableTypeOnFakedType
            ),
        };
    }
}
