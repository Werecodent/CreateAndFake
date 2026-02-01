using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Handlers;

/// <inheritdoc cref="ICopyHandler"/>
/// <param name="factory">Behavior handling cloning of the supported type.</param>
internal sealed class FactoryCopyHandler(
    Type supportedType,
    Func<object, IDuplicatorChainer, object> factory
) : ICopyHandler
{
    /// <inheritdoc/>
    public Type SupportedType { get; } = supportedType;

    /// <inheritdoc/>
    public object? CopySupported(object source, IDuplicatorChainer duplicator)
    {
        return factory.Invoke(source, duplicator);
    }
}
