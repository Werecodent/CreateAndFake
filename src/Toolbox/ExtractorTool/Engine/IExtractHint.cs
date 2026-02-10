using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ExtractorTool.Engine;

/// <summary>Handles extracting specific types for <see cref="IExtractor"/>.</summary>
public interface IExtractHint : IToolHint
{
    /// <summary>Tries to extract content.</summary>
    /// <param name="value">Instance to extract content from.</param>
    /// <param name="extractor">Extractor to use.</param>
    /// <returns>Hint result.</returns>
    ExtractHintResult TryExtract(object? value, IExtractorChainer extractor);
}
