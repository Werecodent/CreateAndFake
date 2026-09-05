using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.NUnit.v3;

/// <inheritdoc/>
public sealed class SizeAttribute : BaseSizeAttribute
{
    /// <inheritdoc/>
    public SizeAttribute(int min, int max)
        : base(min, max) { }

    /// <inheritdoc/>
    public SizeAttribute(int count)
        : base(count) { }
}
