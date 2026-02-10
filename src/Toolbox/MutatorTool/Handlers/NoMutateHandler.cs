using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.MutatorTool.Handlers;

/// <summary>Prevents mutations of the given <paramref name="type"/>.</summary>
/// <param name="type">Specific <see cref="Type"/> to prevent mutations of.</param>
internal sealed class NoMutateHandler(Type type) : IMutateHandler
{
    /// <inheritdoc/>
    public Type? SupportedType { get; } = type;

    /// <inheritdoc/>
    public bool ModifySupported(object instance, IMutatorChainer chainer)
    {
        return false;
    }
}
