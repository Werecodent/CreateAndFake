namespace Werecodent.CreateAndFake.Design.Randomization.Handlers;

/// <summary>Handles randomizing <see cref="IntPtr"/> values.</summary>
/// <param name="size">The size of a signed integer in the executing process, measured in bytes.</param>
internal sealed class IntPtrValueHandler(int size) : IValueHandler
{
    /// <inheritdoc/>
    public Type? SupportedType { get; } = typeof(IntPtr);

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen)
    {
        if (size == 4)
        {
            return new IntPtr(gen.Next<int>());
        }
        else
        {
            return new IntPtr(gen.Next<long>());
        }
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object max)
    {
        return CreateSupported(gen, (IntPtr)default, max);
    }

    /// <inheritdoc/>
    public object CreateSupported(IRandom gen, object min, object max)
    {
        if (size == 4)
        {
            return new IntPtr(gen.Next(((IntPtr)min).ToInt32(), ((IntPtr)max).ToInt32()));
        }
        else
        {
            return new IntPtr(gen.Next(((IntPtr)min).ToInt64(), ((IntPtr)max).ToInt64()));
        }
    }
}
