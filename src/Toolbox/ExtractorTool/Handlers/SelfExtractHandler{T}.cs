using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Handlers;

/// <summary>Prevents further extraction of <typeparamref name="T"/> <see cref="Type"/>s.</summary>
/// <typeparam name="T"><inheritdoc cref="SupportedType" path="/summary"/></typeparam>
internal sealed class SelfExtractHandler<T> : IExtractHandler
{
    /// <inheritdoc/>
    public Type? SupportedType => typeof(T);

    /// <inheritdoc/>
    public bool ExtractSupported(object source, IExtractorChainer chainer)
    {
        return chainer.AddFoundValue(source);
    }
}
