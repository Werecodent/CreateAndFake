using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.FakerTool;

/// <summary>Configuration for controlling faking behavior.</summary>
public sealed record FakerOptions : IToolOptions
{
    /// <summary>Value generator used for base randomization.</summary>
    public required IRandom Gen { get; init; }

    /// <summary>Handles comparisons.</summary>
    public required IValuer? Valuer { get; init; }
}
