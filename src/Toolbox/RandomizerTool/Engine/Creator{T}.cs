namespace CreateAndFake.RandomizerTool.Engine;

/// <inheritdoc cref="ICreator"/>
/// <typeparam name="T"><inheritdoc cref="SupportedType" path="/summary"/></typeparam>
/// <param name="factory">Behavior handling creation of the supported type.</param>
internal sealed class Creator<T>(Func<IRandomizerChainer, T> factory) : ICreator
{
    /// <inheritdoc/>
    public Type SupportedType { get; } = typeof(T);

    /// <inheritdoc/>
    public object? CreateSupported(IRandomizerChainer randomizer)
    {
        return factory.Invoke(randomizer);
    }
}
