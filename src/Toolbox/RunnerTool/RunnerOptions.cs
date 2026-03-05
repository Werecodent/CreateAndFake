using System.Collections.Immutable;
using System.Reflection;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Tooling;
using CreateAndFake.DuplicatorTool;
using CreateAndFake.FakerTool;
using CreateAndFake.MutatorTool;
using CreateAndFake.Properties;
using CreateAndFake.RandomizerTool;
using CreateAndFake.ValuerTool;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.RunnerTool;

/// <summary>Configuration for controlling run behavior.</summary>
public sealed record RunnerOptions : IToolOptions
{
    /// <inheritdoc/>
    public required IRandom Gen { get; init; }

    /// <summary>Handles randomization.</summary>
    public required IRandomizer Randomizer { get; init; }

    /// <summary>Handles object variance.</summary>
    public required IMutator Mutator { get; init; }

    /// <summary>Handles cloning.</summary>
    public required IDuplicator Duplicator { get; init; }

    /// <summary>Provides stubs.</summary>
    public required IFaker Faker { get; init; }

    /// <summary>Provides equality.</summary>
    public required IValuer Valuer { get; init; }

    /// <summary>Attaches <see cref="IReflectableType"/> when faking <see cref="Type"/>s.</summary>
    [ConfigurableOption]
    public bool InheritIReflectableTypeOnFakedType { get; init; } = false;

    /// <summary>Option for which methods to include.</summary>
    [ConfigurableOption]
    public bool IncludeFinalize { get; init; } = false;

    /// <summary>Option for which methods to include.</summary>
    [ConfigurableOption]
    public bool IncludeDispose { get; init; } = false;

    /// <summary>Option for which methods to include.</summary>
    [ConfigurableOption]
    public bool IncludeStaticMethods { get; init; } = true;

    /// <summary>Option for which methods to include.</summary>
    [ConfigurableOption]
    public bool IncludeInstanceMethods { get; init; } = true;

    /// <summary>How long to wait for methods to complete.</summary>
    [ConfigurableOption]
    public TimeSpan Timeout { get; init; } = new(0, 0, 30);

    /// <summary>Values to inject into called methods.</summary>
    public ImmutableArray<object?> InjectionValues { get; init; } = [];

    /// <summary>
    ///     Creates options from <see langword="this"/>
    ///     overridden with values from <paramref name="config"/>.
    /// </summary>
    /// <param name="config">Configuration with overrides to use.</param>
    /// <returns>The created options.</returns>
    internal RunnerOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(nameof(Runner));
        if (section == null)
        {
            return this;
        }

        return this with
        {
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

    /// <inheritdoc/>
    public override string ToString()
    {
        return nameof(RunnerOptions);
    }
}
