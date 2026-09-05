using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Handlers;

/// <inheritdoc cref="IExtractHandler"/>
/// <param name="type"><inheritdoc cref="SupportedType" path="/summary"/></param>
/// <param name="factory">Behavior handling extraction of the supported type.</param>
internal sealed class ConvertExtractHandler(Type type, Func<object, ICollection<object?>> factory)
    : IExtractHandler
{
    /// <inheritdoc/>
    public Type? SupportedType { get; } = type;

    /// <inheritdoc/>
    public bool ExtractSupported(object source, IExtractorChainer chainer)
    {
        if (chainer.AddFoundValue(source))
        {
            foreach (object? item in factory.Invoke(source))
            {
                if (item != null)
                {
                    _ = chainer.InnerExtract(item);
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ExtractSupportedAsync(
        object source,
        IExtractorChainer chainer,
        CancellationToken canceler
    )
    {
        if (await chainer.AddFoundValueAsync(source, canceler).ConfigureAwait(false))
        {
            foreach (object? item in factory.Invoke(source))
            {
                if (item != null)
                {
                    _ = await chainer.InnerExtractAsync(item, canceler).ConfigureAwait(false);
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
