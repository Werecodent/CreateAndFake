namespace CreateAndFake.Design.Randomization.Handlers;

/// <inheritdoc/>
/// <param name="byteSize">Size of <typeparamref name="T"/> in bytes.</param>
/// <param name="bitFactory">Converts random bytes to <typeparamref name="T"/>.</param>
/// <remarks>Not to be used for floating-point numeric types.</remarks>
internal sealed class IntegralValueHandler<T>(short byteSize, Func<byte[], int, T> bitFactory)
    : IValueHandler
    where T : struct, IComparable, IComparable<T>, IEquatable<T>
{
    /// <inheritdoc/>
    public Type? SupportedType => typeof(T);

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        return bitFactory.Invoke(gen.NextBytes(byteSize), 0);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        return (T)Math.Floor(gen.NextPercent() * (dynamic)max);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        T result = (T)Math.Floor(gen.NextPercent() * (1.0 + (dynamic)max - (T)min) + (T)min);

        // Algorithm can rarely produce an overflow in .NET 4.8.
        return result.CompareTo(min) > 0 ? result : min;
    }
}
