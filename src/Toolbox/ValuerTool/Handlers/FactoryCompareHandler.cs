using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Handlers;

/// <inheritdoc cref="ICompareHandler"/>
/// <param name="compareFactory">Behavior handling comparisons of the supported type.</param>
/// <param name="hashFactory">Behavior handling hashing of the supported type.</param>
internal sealed class FactoryCompareHandler(
    Type supportedType,
    Func<object, object, IValuerChainer, IEnumerable<Difference>> compareFactory,
    Func<object, IValuerChainer, int> hashFactory
) : ICompareHandler
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
        return compareFactory.Invoke(expected, actual, valuer);
    }

    public int HashSupported(object item, IValuerChainer valuer)
    {
        return hashFactory.Invoke(item, valuer);
    }
}
