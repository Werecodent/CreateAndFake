using CreateAndFake.Design.Content;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Toolbox.ExtractorTool;
using CreateAndFake.Toolbox.ValuerTool;

namespace CreateAndFake.Toolbox.AsserterTool;

/// <summary>Configuration for controlling assert behavior.</summary>
public record AsserterOptions : IToolOptions
{
    /// <summary>Core randomizer with a potential seed for logging.</summary>
    public required IRandom Gen { get; init; }

    /// <summary>Handles context extraction for comparisons.</summary>
    public required IExtractor Extractor { get; init; }

    /// <summary>Handles comparisons for assertion checks.</summary>
    public required IValuer Valuer { get; init; }

    /// <summary>Options to use when performing <c>String</c> comparison (such as ignoring case or symbols).</summary>
    public StringComparison StringCompareOption { get; init; } = StringComparison.InvariantCulture;
}