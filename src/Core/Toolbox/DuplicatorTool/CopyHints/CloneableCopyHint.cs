using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.DuplicatorTool.CopyHints;

/// <summary>Handles cloning <see cref="ICloneable"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class CloneableCopyHint : CopyHint<ICloneable>
{
    /// <inheritdoc/>
    protected override ICloneable Copy(ICloneable source, DuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, nameof(source));

        return (ICloneable)source.Clone();
    }
}
