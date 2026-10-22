using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.ExtractorTool.Engine;

/// <summary>Handles extraction of the <see cref="ITypeSupporter.SupportedType"/>.</summary>
public interface IExtractHandler : ITypeSupporter
{
    /// <inheritdoc cref="ExtractHint.TryToExtract(object,IExtractorChainer)"/>
    bool ExtractSupported(object source, IExtractorChainer chainer);

    /// <inheritdoc cref="ExtractHint.TryToExtractAsync(object,IExtractorChainer,CancellationToken)"/>
    Task<bool> ExtractSupportedAsync(
        object source,
        IExtractorChainer chainer,
        CancellationToken canceler
    );
}
