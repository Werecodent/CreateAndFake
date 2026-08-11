using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.xUnit.v3;

/// <inheritdoc/>
public sealed class CapAttribute<T> : BaseCapAttribute<T>
    where T : struct, IComparable, IComparable<T>, IEquatable<T>
{
    /// <inheritdoc/>
    public CapAttribute(T min, T max)
        : base(min, max) { }

    /// <inheritdoc/>
    public CapAttribute(T max)
        : base(max) { }
}
