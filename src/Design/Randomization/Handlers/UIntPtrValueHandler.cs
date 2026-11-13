namespace Werecodent.CreateAndFake.Design.Randomization.Handlers;

/// <inheritdoc/>
internal sealed class UIntPtrValueHandler(int size) : IValueHandler
{
    /// <inheritdoc/>
    public Type? SupportedType { get; } = typeof(UIntPtr);

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        if (size == 4)
        {
            return new UIntPtr(gen.Next<uint>());
        }
        else
        {
            return new UIntPtr(gen.Next<ulong>());
        }
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        return CreateSupported(gen, (UIntPtr)default, max);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        if (size == 4)
        {
            return new UIntPtr(gen.Next(((UIntPtr)min).ToUInt32(), ((UIntPtr)max).ToUInt32()));
        }
        else
        {
            return new UIntPtr(gen.Next(((UIntPtr)min).ToUInt64(), ((UIntPtr)max).ToUInt64()));
        }
    }
}
