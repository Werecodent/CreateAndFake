using Werecodent.CreateAndFake.MutatorTool.Engine;

namespace Werecodent.CreateAndFake.MutatorTool.Handlers;

/// <summary>
///     Handles <typeparamref name="T"/> mutations via the <paramref name="factory"/>.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="SupportedType" path="/summary"/></typeparam>
/// <param name="factory">Behavior handling <typeparamref name="T"/> mutations.</param>
internal sealed class FactoryMutateHandler<T>(Action<T, IMutatorChainer> factory) : IMutateHandler
{
    /// <inheritdoc/>
    public Type? SupportedType { get; } = typeof(T);

    /// <inheritdoc/>
    public bool ModifySupported(object instance, IMutatorChainer chainer)
    {
        factory.Invoke((T)instance, chainer);
        return true;
    }
}
