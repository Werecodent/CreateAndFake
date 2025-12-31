using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool;
using CreateAndFake.RandomizerTool;
using CreateAndFake.ValuerTool;
using Microsoft.Extensions.Configuration;

namespace CreateAndFake.MutatorTool;

/// <summary>Configuration for controlling mutating behavior.</summary>
public sealed record MutatorOptions : IToolOptions
{
    /// <summary>Value generator used for base randomization.</summary>
    public required IRandom Gen { get; init; }

    /// <summary>Handles randomization.</summary>
    public required IRandomizer Randomizer { get; init; }

    /// <summary>Ensures object variance.</summary>
    public required IValuer Valuer { get; init; }

    /// <summary>Handles value extraction.</summary>
    public required IExtractor Extractor { get; init; }

    /// <summary>Limits attempts at creating variants.</summary>
    [ConfigurableOption]
    public Limiter Limiter { get; init; } = Limiter.Score;

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
            Limiter = section.GetValue(nameof(Limiter), Limiter),
        };
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return nameof(MutatorOptions);
    }
}
