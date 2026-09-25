using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.ExtractorTool.Engine;

/// <summary>Handles extracting specific types for <see cref="IExtractor"/>.</summary>
public interface IExtractHint : IToolHint
{
    /// <inheritdoc cref="TryToExtractAsync"/>
    ExtractHintResult TryToExtract(object? source, IExtractorChainer chainer);

    /// <summary>Tries to extract content.</summary>
    /// <param name="source">Instance to extract content from.</param>
    /// <param name="chainer">Extractor to use.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>Hint result.</returns>
    Task<ExtractHintResult> TryToExtractAsync(
        object? source,
        IExtractorChainer chainer,
        CancellationToken canceler
    );
}
