using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting <see langword="null"/> values for <see cref="IExtractor"/>.</summary>
public sealed class NullExtractHint : ExtractHint
{
    /// <inheritdoc/>
    public override ExtractHintResult TryExtract(object? value, IExtractorChainer extractor)
    {
        if (value == null)
        {
            return new(false);
        }
        else
        {
            return ExtractHintResult.None;
        }
    }
}
