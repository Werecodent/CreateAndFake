namespace CreateAndFake.Design.Randomization;

/// <summary>Handles randomizing <typeparamref name="T"/> values.</summary>
/// <typeparam name="T">The value <see cref="Type"/> to handle.</typeparam>
/// <param name="createFactory">
///     Behavior for <see cref="CreateSupported(IRandom)"/>.
/// </param>
/// <param name="constrainedFactory">
///     Behavior for <see cref="CreateSupported(object,object,double)"/>.
/// </param>
internal sealed class ValueHandler<T>(
    Func<IRandom, T> createFactory,
    Func<T, T, double, T> constrainedFactory
) : IValueHandler
{
    /// <inheritdoc/>
    public Type? SupportedType { get; } = typeof(T);

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        return createFactory(gen)!;
    }

    /// <inheritdoc/>
    public object CreateSupported(object min, object max, double percent)
    {
        return constrainedFactory((T)min, (T)max, percent)!;
    }
}
