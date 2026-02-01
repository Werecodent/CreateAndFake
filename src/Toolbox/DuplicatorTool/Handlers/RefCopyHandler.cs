using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Handlers;

/// <summary>...</summary>
/// <param name="supportedType"></param>
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
