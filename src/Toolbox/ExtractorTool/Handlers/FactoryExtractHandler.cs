using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Handlers;

/// <inheritdoc cref="IExtractHandler"/>
/// <typeparam name="T"><inheritdoc cref="SupportedType" path="/summary"/></typeparam>
/// <param name="factory">Behavior handling extraction of the supported type.</param>
internal sealed class FactoryExtractHandler<T>(Func<T, IEnumerable<object?>> factory)
    : IExtractHandler
{
    /// <inheritdoc/>
    public Type? SupportedType { get; } = typeof(T);

    /// <inheritdoc/>
    public bool ExtractSupported(object source, IExtractorChainer chainer)
    {
        if (chainer.AddFoundValue(source))
        {
            int i = 0;
            foreach (object? item in factory.Invoke((T)source))
            {
                ArgumentGuard.ThrowUponIterationLimit(
                    i++,
                    chainer.Options.Valuer.Options.IterationLimit
                );

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
}
