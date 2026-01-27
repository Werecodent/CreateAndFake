namespace CreateAndFake.Design.Randomization.Handlers;

/// <summary>Handles randomizing <typeparamref name="T"/> values.</summary>
/// <typeparam name="T">The value <see cref="Type"/> to handle.</typeparam>
internal abstract class ValueHandler<T> : IValueHandler
    where T : struct, IComparable, IComparable<T>, IEquatable<T>
{
    /// <inheritdoc/>
    public Type? SupportedType => typeof(T);

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        return Create(gen)!;
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        return CreateSupported(gen, (T)default, max);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        return Create(gen, (T)min, (T)max)!;
    }

    /// <inheritdoc cref="CreateSupported(IRandom)"/>
    protected abstract T Create(IRandom gen);

    /// <inheritdoc cref="CreateSupported(IRandom,object,object)"/>
    protected abstract T Create(IRandom gen, T min, T max);
}
