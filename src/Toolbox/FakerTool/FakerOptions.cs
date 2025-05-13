using CreateAndFake.Design.Tooling;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.FakerTool;

/// <summary>Configuration for controlling faking behavior.</summary>
public record FakerOptions : IToolOptions
{
    /// <summary>Handles comparisons.</summary>
    public required IValuer? Valuer { get; init; }
}
