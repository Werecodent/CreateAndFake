using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Handlers;

/// <inheritdoc cref="ICompareHandler"/>
internal sealed class DefaultEqualityCompareHandler(Type supportedType) : ICompareHandler
{
    /// <inheritdoc/>
    public Type SupportedType { get; } = supportedType;

    /// <inheritdoc/>
    public IEnumerable<Difference> CompareSupported(
        object expected,
        object actual,
        IValuerChainer chainer
    )
    {
        ArgumentGuard.ThrowIfNull(expected);

        if (!expected.Equals(actual))
        {
            yield return new Difference(".equals", new Difference(expected, actual));
        }
    }

    /// <inheritdoc/>
    public int HashSupported(object item, IValuerChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(item);

        return item.GetHashCode();
    }
}
