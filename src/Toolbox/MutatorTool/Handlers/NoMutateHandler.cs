using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.MutatorTool.Handlers;

/// <inheritdoc cref="IMutateHandler"/>
internal sealed class NoMutateHandler(Type type) : IMutateHandler
{
    /// <inheritdoc/>
    public Type? SupportedType { get; } = type;

    /// <inheritdoc/>
    public bool ModifySupported(object instance, IMutatorChainer mutator)
    {
        return false;
    }
}
