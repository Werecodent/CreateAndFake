using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning <see cref="IDeepCloneable"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class DeepCloneableCopyHint : CopyHint<IDeepCloneable>
{
    /// <inheritdoc/>
    protected override IDeepCloneable Copy(IDeepCloneable source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, nameof(source));

        return source.DeepClone();
    }
}
