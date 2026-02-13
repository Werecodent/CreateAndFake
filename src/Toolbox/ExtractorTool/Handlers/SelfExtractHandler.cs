using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Handlers;

/// <summary>Prevents further extraction of the <paramref name="supportedType"/>.</summary>
/// <param name="supportedType"><inheritdoc cref="SupportedType" path="/summary"/></param>
internal sealed class SelfExtractHandler(Type supportedType) : IExtractHandler
{
    /// <inheritdoc/>
    public Type? SupportedType => supportedType;

    /// <inheritdoc/>
    public bool ExtractSupported(object source, IExtractorChainer chainer)
    {
        return chainer.AddFoundValue(source);
    }
}
