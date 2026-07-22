using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.FakerTool;

#pragma warning disable CA1051, S1104 // Required to match out/ref.

/// <summary>For matching <see langword="out"/> and <see langword="ref"/> arguments.</summary>
/// <typeparam name="T">Argument <see cref="Type"/> to match.</typeparam>
public sealed class OutRef<T> : IOutRef
{
    /// <summary>Used as the out/ref argument.</summary>
    public T? Var = default;
}

#pragma warning restore
