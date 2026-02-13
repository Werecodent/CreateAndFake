using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Handlers;

/// <summary>Prevents cloning of the <paramref name="supportedType"/>.</summary>
/// <param name="supportedType">Specific <see cref="Type"/> to prevent cloning of.</param>
internal sealed class RefCopyHandler(Type supportedType) : ICopyHandler
{
    /// <inheritdoc/>
    public Type SupportedType { get; } = supportedType;

    /// <inheritdoc/>
    public object? CopySupported(object source, IDuplicatorChainer duplicator)
    {
        return source;
    }
}
