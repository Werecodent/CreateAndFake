using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Hints;

/// <summary>Handles comparing objects for <see cref="IValuer"/>.</summary>
public sealed class PublicObjectCompareHint() : ObjectCompareHint(true)
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.PublicObjectHint;
}
