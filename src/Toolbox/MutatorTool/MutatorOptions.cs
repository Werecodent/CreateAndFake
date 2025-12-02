using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool;
using CreateAndFake.RandomizerTool;
using CreateAndFake.ValuerTool;

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
    public Limiter Limiter { get; init; } = Limiter.Score;

    /// <inheritdoc/>
    public override string ToString()
    {
        return nameof(MutatorOptions);
    }
}
