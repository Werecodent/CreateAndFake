using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Handlers;

/// <inheritdoc cref="ICompareHandler"/>
/// <param name="converter">Behavior handling comparisons of the supported type.</param>
internal sealed class ConvertCompareHandler<T>(Func<T, IValuerChainer, object?> converter)
    : ICompareHandler
{
    /// <inheritdoc/>
    public Type SupportedType { get; } = typeof(T);

    /// <inheritdoc/>
    public IEnumerable<Difference> CompareSupported(
        object expected,
        object actual,
        IValuerChainer chainer
    )
    {
        ArgumentGuard.ThrowIfNull(converter, expected, actual, chainer);

        return chainer.Compare(converter((T)expected, chainer), converter((T)actual, chainer));
    }

    /// <inheritdoc/>
    public int HashSupported(object item, IValuerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(converter, item, chainer);

        return chainer.GetHashCode(converter((T)item, chainer));
    }
}
