namespace CreateAndFake.DuplicatorTool.Engine;

/// <inheritdoc cref="ICopier"/>
/// <param name="factory">Behavior handling cloning of the supported type.</param>
internal sealed class Copier(Type supportedType, Func<object, IDuplicatorChainer, object> factory)
    : ICopier
{
    /// <inheritdoc/>
    public Type SupportedType { get; } = supportedType;

    /// <inheritdoc/>
    public object? CopySupported(object source, IDuplicatorChainer duplicator)
    {
        return factory.Invoke(source, duplicator);
    }
}
