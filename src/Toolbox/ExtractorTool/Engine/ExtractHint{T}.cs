using Werecodent.CreateAndFake.Design;

namespace Werecodent.CreateAndFake.ExtractorTool.Engine;

/// <typeparam name="T"><see cref="Type"/> being supported for extraction.</typeparam>
/// <inheritdoc/>
public abstract class ExtractHint<T> : ExtractHint
{
    /// <inheritdoc/>
    public sealed override ExtractHintResult TryToExtract(object? source, IExtractorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (source is T supported)
        {
            return new(Extract(supported, chainer));
        }
        else
        {
            return ExtractHintResult.None;
        }
    }

    /// <inheritdoc cref="TryToExtract"/>
    protected abstract bool Extract(T source, IExtractorChainer chainer);
}
