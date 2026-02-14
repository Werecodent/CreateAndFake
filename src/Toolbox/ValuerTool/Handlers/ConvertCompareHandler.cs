using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Handlers;

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
        IValuerChainer valuer
    )
    {
        return valuer.Compare(converter((T)expected, valuer), converter((T)actual, valuer));
    }

    /// <inheritdoc/>
    public int HashSupported(object item, IValuerChainer valuer)
    {
        return valuer.GetHashCode(converter((T)item, valuer));
    }
}
