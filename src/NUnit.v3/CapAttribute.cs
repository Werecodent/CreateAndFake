using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.NUnit.v3;

/// <inheritdoc/>
public sealed class CapAttribute : BaseCapAttribute
{
    /// <inheritdoc/>
    public CapAttribute(object min, object max)
        : base(min, max) { }

    /// <inheritdoc/>
    public CapAttribute(object max)
        : base(max) { }

    /// <inheritdoc/>
    public override object Clone()
    {
        return IsMinSet ? new CapAttribute(Min, Max) : new CapAttribute(Max);
    }
}
