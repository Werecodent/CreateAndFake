using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.NUnit.v3;

/// <inheritdoc/>
public sealed class CapAttribute : BaseCapAttribute
{
    /// <inheritdoc/>
    public CapAttribute(int min, int max)
        : base(min, max) { }

    /// <inheritdoc/>
    public CapAttribute(int max)
        : base(max) { }
}
