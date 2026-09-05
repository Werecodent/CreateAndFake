using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.MSTest.v4;

/// <inheritdoc/>
public sealed class CapAttribute : BaseCapAttribute
{
    /// <inheritdoc/>
    public CapAttribute(object min, object max)
        : base(min, max) { }

    /// <inheritdoc/>
    public CapAttribute(object max)
        : base(max) { }
}
