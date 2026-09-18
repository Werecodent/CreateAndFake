using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.RandomizerTool.Engine;

namespace Werecodent.CreateAndFake.RandomizerTool.Handlers;

/// <inheritdoc cref="ICreateHandler"/>
/// <typeparam name="T"><inheritdoc cref="SupportedType" path="/summary"/></typeparam>
/// <param name="factory">Behavior handling creation of the supported type.</param>
internal sealed class FactoryCreateHandler<T>(Func<IRandomizerChainer, T> factory) : ICreateHandler
{
    /// <inheritdoc/>
    public Type SupportedType { get; } = typeof(T);

    /// <inheritdoc/>
    public object? CreateSupported(IRandomizerChainer randomizer)
    {
        return factory.Invoke(randomizer);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}
