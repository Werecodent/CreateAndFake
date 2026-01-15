namespace CreateAndFake.DuplicatorTool.Engine;

/// <inheritdoc cref="ICopier"/>
/// <typeparam name="T"><inheritdoc cref="SupportedType" path="/summary"/></typeparam>
/// <param name="factory">Behavior handling creation of the supported type.</param>
internal sealed class Copier<T>(Func<T, IDuplicatorChainer, T> factory) : ICopier
{
    /// <inheritdoc/>
    public Type SupportedType { get; } = typeof(T);

    /// <inheritdoc/>
    public object? CopySupported(object source, IDuplicatorChainer duplicator)
    {
        return factory.Invoke((T)source, duplicator);
    }
}
