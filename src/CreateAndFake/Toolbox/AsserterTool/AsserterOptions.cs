using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Toolbox.ValuerTool;

namespace CreateAndFake.Toolbox.AsserterTool;

/// <summary>Configuration for controlling assert behavior.</summary>
public record AsserterOptions : IToolOptions
{
    /// <summary>Core randomizer with a potential seed for logging.</summary>
    public required IRandom Gen { get; init; }

    /// <summary>Handles comparisons for assertion checks.</summary>
    public required IValuer Valuer { get; init; }
}