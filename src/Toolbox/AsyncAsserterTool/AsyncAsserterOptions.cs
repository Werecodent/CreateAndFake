using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Design.Tooling;
using CreateAndFake.ExtractorTool;
using CreateAndFake.ValuerTool;

namespace CreateAndFake.AsyncAsserterTool;

/// <summary>Configuration for controlling assert behavior.</summary>
public record AsyncAsserterOptions : IToolOptions
{
    /// <summary>Handles common test scenarios.</summary>
    public required IAsserter Asserter { get; init; }

    /// <summary>Core randomizer with a potential seed for logging.</summary>
    public required IRandom Gen { get; init; }

    /// <summary>Handles context extraction for comparisons.</summary>
    public required IExtractor Extractor { get; init; }

    /// <summary>Handles comparisons for assertion checks.</summary>
    public required IValuer Valuer { get; init; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return nameof(AsyncAsserterOptions);
    }
}
