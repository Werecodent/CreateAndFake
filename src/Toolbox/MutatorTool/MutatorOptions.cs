using Microsoft.Extensions.Configuration;
using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.Design.Tooling;
using Werecodent.CreateAndFake.ExtractorTool;
using Werecodent.CreateAndFake.MutatorTool.Engine;
using Werecodent.CreateAndFake.Properties;
using Werecodent.CreateAndFake.RandomizerTool;
using Werecodent.CreateAndFake.ValuerTool;

namespace Werecodent.CreateAndFake.MutatorTool;

/// <summary>Configuration for manipulating <see cref="IMutator"/> behavior.</summary>
public sealed record MutatorOptions : ToolHintOptions<MutatorOptions, IMutateHint>
{
    /// <summary>Identifies the configuration section containing these settings.</summary>
    internal static string ConfigSectionName { get; } = nameof(Mutator);

    /// <inheritdoc cref="IRandomizer"/>
    public required IRandomizer Randomizer { get; init; }

    /// <inheritdoc cref="IValuer"/>
    public required IValuer Valuer { get; init; }

    /// <inheritdoc cref="IExtractor"/>
    public required IExtractor Extractor { get; init; }

    /// <summary>Limits attempts at creating variants.</summary>
    /// <remarks>Attempts fail due to the expected variant actually being equal to the original.</remarks>
    /// <value><c>Limiter.Score</c></value>
    [ConfigurableOption]
    public ILimiter AttemptLimitForVariants { get; init; } = Limiter.Score;

    /// <summary>Limits attempts at creating uniques.</summary>
    /// <remarks>Attempts fail due to the expected unique actually sharing data with the original.</remarks>
    /// <value><c>Limiter.Score</c></value>
    [ConfigurableOption]
    public ILimiter AttemptLimitForUniques { get; init; } = Limiter.Score;

    /// <summary>Limits attempts at mutating sets.</summary>
    /// <remarks>Attempts fail due to randomized values already existing in the set.</remarks>
    /// <value><c>6</c></value>
    [ConfigurableOption]
    public int MaxAttemptsForMutatingSets { get; init; } = 6;

    /// <summary>Derives options from <see langword="this"/> overridden by <paramref name="config"/>'s values.</summary>
    /// <param name="config">Configuration with the potential overrides to use.</param>
    /// <returns>The created options.</returns>
    internal MutatorOptions WithConfig(IConfigurationSection? config)
    {
        IConfigurationSection? section = config?.GetSection(ConfigSectionName);
        if (section == null)
        {
            return this;
        }

        return this with
        {
            MaxAttemptsForMutatingSets = Config.GetValue(section, MaxAttemptsForMutatingSets),
            AttemptLimitForVariants = Config.GetValue(section, AttemptLimitForVariants),
            AttemptLimitForUniques = Config.GetValue(section, AttemptLimitForUniques),
            IncludeFrameworkHints = Config.GetValue(section, IncludeFrameworkHints),
            IncludeFoundHints = Config.GetValue(section, IncludeFoundHints),
            MaxRecursionDepth = Config.GetValue(section, MaxRecursionDepth),
        };
    }
}
