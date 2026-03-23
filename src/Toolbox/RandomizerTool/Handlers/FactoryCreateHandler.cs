using CreateAndFake.Design.Types;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Handlers;

/// <inheritdoc cref="ICreateHandler"/>
/// <param name="factory">Behavior handling creation of the supported type.</param>
internal sealed class FactoryCreateHandler(
    Type supportedType,
    Func<IRandomizerChainer, object?> factory
) : ICreateHandler
{
    /// <inheritdoc/>
    public Type SupportedType { get; } = supportedType;

    /// <inheritdoc/>
    public object? CreateSupported(IRandomizerChainer randomizer)
    {
        return factory?.Invoke(randomizer);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{nameof(FactoryCreateHandler)}({TypeHelper.ExpandedName(SupportedType)})";
    }
}
