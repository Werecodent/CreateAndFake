using CreateAndFake.Design;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool;
using CreateAndFake.RandomizerTool;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.MutatorTool;

/// <summary>Configuration for controlling mutating behavior.</summary>
public record MutatorOptions : IToolOptions
{
    /// <summary>Handles randomization.</summary>
    public required IRandomizer Randomizer { get; init; }

    /// <summary>Ensures object variance.</summary>
    public required IValuer Valuer { get; init; }

    /// <summary>Handles value extraction.</summary>
    public required IExtractor Extractor { get; init; }

    /// <summary>Limits attempts at creating variants.</summary>
    public Limiter Limiter { get; init; } = Limiter.Score;
}
