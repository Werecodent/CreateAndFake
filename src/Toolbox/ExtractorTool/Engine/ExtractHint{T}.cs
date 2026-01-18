using CreateAndFake.Design;

namespace CreateAndFake.ExtractorTool.Engine;

/// <typeparam name="T"><see cref="Type"/> being supported for extraction.</typeparam>
/// <inheritdoc/>
public abstract class ExtractHint<T> : ExtractHint
{
    /// <inheritdoc/>
    public sealed override ExtractHintResult TryExtract(object? value, IExtractorChainer extractor)
    {
        ArgumentGuard.ThrowIfNull(extractor);

        if (value is T supported)
        {
            return new(Extract(supported, extractor));
        }
        else
        {
            return ExtractHintResult.None;
        }
    }

    /// <inheritdoc cref="TryExtract"/>
    protected abstract bool Extract(T value, IExtractorChainer extractor);
}
