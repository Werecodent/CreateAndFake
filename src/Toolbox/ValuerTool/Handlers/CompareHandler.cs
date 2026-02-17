using CreateAndFake.ValuerTool.Engine;

namespace CreateAndFake.ValuerTool.Handlers;

/// <summary>
///     Handles comparison and hashing of <typeparamref name="T"/> <see cref="Type"/>s.
/// </summary>
/// <typeparam name="T">The supported <see cref="Type"/>.</typeparam>
internal abstract class CompareHandler<T> : ICompareHandler
{
    /// <inheritdoc/>
    public Type? SupportedType => typeof(T);

    /// <inheritdoc/>
    public IEnumerable<Difference> CompareSupported(
        object? expected,
        object? actual,
        IValuerChainer chainer
    )
    {
        return Compare((T)expected!, (T)actual!, chainer);
    }

    /// <inheritdoc cref="CompareSupported"/>
    protected abstract IEnumerable<Difference> Compare(
        T expected,
        T actual,
        IValuerChainer chainer
    );

    /// <inheritdoc/>
    public int HashSupported(object? item, IValuerChainer chainer)
    {
        return Hash((T)item!, chainer);
    }

    /// <inheritdoc cref="HashSupported"/>
    protected abstract int Hash(T item, IValuerChainer chainer);
}
