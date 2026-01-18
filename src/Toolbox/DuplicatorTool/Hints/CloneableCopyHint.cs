using CreateAndFake.Design;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning <see cref="ICloneable"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class CloneableCopyHint : CopyHint<ICloneable>
{
    /// <inheritdoc/>
    protected override ICloneable Copy(ICloneable source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source);

        return (ICloneable)source.Clone();
    }
}
