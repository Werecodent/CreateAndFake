using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.MutatorTool.Handlers;

/// <inheritdoc cref="IMutateHandler"/>
/// <typeparam name="T"><inheritdoc cref="SupportedType" path="/summary"/></typeparam>
/// <param name="factory">
///     Behavior handling mutation of the <inheritdoc cref="SupportedType" path="/summary"/>.
/// </param>
internal sealed class FactoryMutateHandler<T>(Action<T, IMutatorChainer> factory) : IMutateHandler
{
    /// <inheritdoc/>
    public Type? SupportedType { get; } = typeof(T);

    /// <inheritdoc/>
    public bool ModifySupported(object instance, IMutatorChainer mutator)
    {
        factory.Invoke((T)instance, mutator);
        return true;
    }
}
