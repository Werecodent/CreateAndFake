using CreateAndFake.Design;
using CreateAndFake.Design.Content;

namespace CreateAndFake.DuplicatorTool.CopyHints;

/// <summary>Handles cloning <see cref="IDeepCloneable"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class DeepCloneableCopyHint : CopyHint<IDeepCloneable>
{
    /// <inheritdoc/>
    protected override IDeepCloneable Copy(IDeepCloneable source, DuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, nameof(source));

        return source.DeepClone();
    }
}
