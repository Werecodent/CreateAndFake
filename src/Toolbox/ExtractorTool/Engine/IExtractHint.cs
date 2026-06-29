using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ExtractorTool.Engine;

/// <summary>Handles extracting specific types for <see cref="IExtractor"/>.</summary>
public interface IExtractHint : IToolHint
{
    /// <summary>Tries to extract content.</summary>
    /// <param name="source">Instance to extract content from.</param>
    /// <param name="chainer">Extractor to use.</param>
    /// <returns>Hint result.</returns>
    ExtractHintResult TryToExtract(object? source, IExtractorChainer chainer);
}
