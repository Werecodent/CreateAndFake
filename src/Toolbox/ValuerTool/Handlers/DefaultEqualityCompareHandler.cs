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
        if (!expected.Equals(actual))
        {
            yield return new Difference(".equals", new Difference(expected, actual));
        }
    }

    /// <inheritdoc/>
    public int HashSupported(object item, IValuerChainer chainer)
    {
        return item.GetHashCode();
    }
}
