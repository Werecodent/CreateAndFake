using CreateAndFake.Design.Content;
using CreateAndFake.RandomizerTool;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.RunnerTool;

/// <summary>Configuration for controlling run behavior.</summary>
public record RunnerOptions : IToolOptions
{
    /// <summary>Handles randomization.</summary>
    public required IRandomizer Randomizer { get; init; }

    /// <summary>Ensures object variance.</summary>
    public required IValuer Valuer { get; init; }
}