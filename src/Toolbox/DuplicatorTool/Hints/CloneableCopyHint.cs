using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning <see cref="ICloneable"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class CloneableCopyHint : CopyHint<ICloneable>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.CloneableHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(ICloneable)];

    /// <inheritdoc/>
    protected override ICloneable Copy(ICloneable source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source);

        return (ICloneable)source.Clone();
    }
}
