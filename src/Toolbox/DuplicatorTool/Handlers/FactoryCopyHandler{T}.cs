using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Handlers;

/// <inheritdoc cref="ICopyHandler"/>
/// <typeparam name="T"><inheritdoc cref="SupportedType" path="/summary"/></typeparam>
/// <param name="factory">Behavior handling creation of the supported type.</param>
internal sealed class FactoryCopyHandler<T>(Func<T, IDuplicatorChainer, T> factory) : ICopyHandler
{
    /// <inheritdoc/>
    public Type SupportedType { get; } = typeof(T);

    /// <inheritdoc/>
    public object? CopySupported(object source, IDuplicatorChainer duplicator)
    {
        return factory.Invoke((T)source, duplicator);
    }
}
