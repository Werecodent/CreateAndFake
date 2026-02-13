using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Handlers;

/// <summary>
///     Handles the <paramref name="supportedType"/> cloning via the <paramref name="factory"/>.
/// </summary>
/// <param name="supportedType"><inheritdoc cref="SupportedType" path="/summary"/></param>
/// <param name="factory">Behavior handling <paramref name="supportedType"/> cloning.</param>
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
