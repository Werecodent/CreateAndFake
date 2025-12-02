using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.AsserterTool;

/// <summary>Configuration for controlling assert behavior.</summary>
public sealed record AsserterOptions : IToolOptions
{
    /// <summary>Core randomizer with a potential seed for logging.</summary>
    public required IRandom Gen { get; init; }

    /// <summary>Handles context extraction for comparisons.</summary>
    public required IExtractor Extractor { get; init; }

    /// <summary>Handles comparisons for assertion checks.</summary>
    public required IValuer Valuer { get; init; }

    /// <summary>Options to use when performing <see langword="string"/> comparison (such as ignoring case or symbols).</summary>
    public StringComparison StringCompareOption { get; init; } = StringComparison.InvariantCulture;

    /// <inheritdoc/>
    public override string ToString()
    {
        return nameof(AsserterOptions);
    }
}
