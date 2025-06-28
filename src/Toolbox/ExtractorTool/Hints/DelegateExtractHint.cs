using CreateAndFake.Design;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting delegates for <see cref="IExtractor"/>.</summary>
public sealed class DelegateExtractHint : ExtractHint<Delegate>
{
    /// <inheritdoc/>
    protected override bool Extract(Delegate value, IExtractorChainer extractor)
    {
        ArgumentGuard.ThrowIfNull(extractor, nameof(extractor));

        return extractor.AddFoundValue(value);
    }
}
