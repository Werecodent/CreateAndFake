using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Handlers;

/// <inheritdoc cref="ICompareHandler"/>
internal sealed class FallbackCompareHandler(Type supportedType) : ICompareHandler
{
    /// <inheritdoc/>
    public Type SupportedType { get; } = supportedType;

    /// <inheritdoc/>
    public IEnumerable<Difference> CompareSupported(
        object expected,
        object actual,
        IValuerChainer valuer
    )
    {
        if (expected != actual)
        {
            yield return new Difference(".equals", new Difference(expected, actual));
        }
    }

    public int HashSupported(object item, IValuerChainer valuer)
    {
        return item.GetHashCode();
    }
}
