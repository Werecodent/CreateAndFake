namespace CreateAndFake.RandomizerTool.Engine;

/// <inheritdoc cref="ICreator"/>
/// <param name="factory">Behavior handling creation of the supported type.</param>
internal sealed class Creator(Type supportedType, Func<IRandomizerChainer, object> factory)
    : ICreator
{
    /// <inheritdoc/>
    public Type SupportedType { get; } = supportedType;

    /// <inheritdoc/>
    public object? CreateSupported(IRandomizerChainer randomizer)
    {
        return factory?.Invoke(randomizer);
    }
}
