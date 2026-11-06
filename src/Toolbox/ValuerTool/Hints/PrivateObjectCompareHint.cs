using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing objects for <see cref="IValuer"/>.</summary>
public sealed class PrivateObjectCompareHint() : ObjectCompareHint(false)
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.PrivateObjectHint;
}
