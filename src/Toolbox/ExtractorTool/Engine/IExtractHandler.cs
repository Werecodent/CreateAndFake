using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.ExtractorTool.Engine;

/// <summary>Handles extraction of the <see cref="ITypeSupporter.SupportedType"/>.</summary>
public interface IExtractHandler : ITypeSupporter
{
    /// <inheritdoc cref="ExtractHint.TryToExtract(object?, IExtractorChainer)"/>
    bool ExtractSupported(object source, IExtractorChainer chainer);
}
