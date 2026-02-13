using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.MutatorTool.Handlers;

/// <summary>Prevents mutations of the <paramref name="supportedType"/>.</summary>
/// <param name="supportedType">Specific <see cref="Type"/> to prevent mutations of.</param>
internal sealed class NoMutateHandler(Type supportedType) : IMutateHandler
{
    /// <inheritdoc/>
    public Type? SupportedType { get; } = supportedType;

    /// <inheritdoc/>
    public bool ModifySupported(object instance, IMutatorChainer chainer)
    {
        return false;
    }
}
