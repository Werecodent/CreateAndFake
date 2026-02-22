using CreateAndFake.Design.Types;

namespace CreateAndFake.ExtractorTool.Engine;

/// <summary>Handles extraction of the <see cref="ITypeSupporter.SupportedType"/>.</summary>
public interface IExtractHandler : ITypeSupporter
{
    /// <inheritdoc cref="ExtractHint.TryExtract(object?, IExtractorChainer)"/>
    bool ExtractSupported(object source, IExtractorChainer chainer);
}
