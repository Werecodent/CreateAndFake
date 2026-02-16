namespace CreateAndFake.Design.Randomization.Handlers;

/// <summary>
///     Handles <typeparamref name="T"/> randomization via the <paramref name="factory"/>.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="SupportedType" path="/summary"/></typeparam>
/// <param name="min"></param>
/// <param name="max"></param>
/// <param name="factory">Behavior handling <typeparamref name="T"/> randomization.</param>
internal sealed class FactoryValueHandler<T>(T min, T max, Func<IRandom, T, T, T> factory)
    : IValueHandler
    where T : struct, IComparable, IComparable<T>, IEquatable<T>
{
    /// <inheritdoc/>
    public Type? SupportedType => typeof(T);

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        return CreateSupported(gen, min, max);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        return CreateSupported(gen, (T)default, max);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        return factory.Invoke(gen, (T)min, (T)max);
    }
}
