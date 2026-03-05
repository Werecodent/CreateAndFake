using CreateAndFake.Design.Reiteration;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.Properties;
using CreateAndFake.RandomizerTool;
using CreateAndFake.ValuerTool;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.MutatorTool;

/// <summary>Configuration for manipulating <see cref="IMutator"/> behavior.</summary>
public sealed record MutatorOptions : ToolHintOptions<MutatorOptions, IMutateHint>
{
    /// <summary>Handles randomization.</summary>
    public required IRandomizer Randomizer { get; init; }

    /// <summary>Handles value comparison to ensure object variance.</summary>
    public required IValuer Valuer { get; init; }

    /// <summary>Handles value extraction for creating unique instances.</summary>
    public required IExtractor Extractor { get; init; }

    /// <summary>Limits attempts at creating variants.</summary>
    [ConfigurableOption]
    public ILimiter CreateVariantAttemptLimit { get; init; } = Limiter.Score;

    /// <summary>Limits attempts at creating uniques.</summary>
    [ConfigurableOption]
    public ILimiter CreateUniqueAttemptLimit { get; init; } = Limiter.Score;

    /// <summary>
    ///     Creates options from <see langword="this"/>
    ///     overridden with values from <paramref name="config"/>.
    /// </summary>
    /// <param name="config">Configuration with overrides to use.</param>
    /// <returns>The created options.</returns>
    internal MutatorOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(nameof(Mutator));
        if (section == null)
        {
            return this;
        }

        return this with
        {
            CreateVariantAttemptLimit = Config.GetValue(section, CreateVariantAttemptLimit),
            CreateUniqueAttemptLimit = Config.GetValue(section, CreateUniqueAttemptLimit),
            IncludeFrameworkHints = Config.GetValue(section, IncludeFrameworkHints),
            IncludeFoundHints = Config.GetValue(section, IncludeFoundHints),
            MaxHintRecursion = Config.GetValue(section, MaxHintRecursion),
        };
    }
}
