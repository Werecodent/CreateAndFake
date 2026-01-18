using CreateAndFake.Design;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning <see cref="IDuplicatable"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class DuplicatableCopyHint : CopyHint<IDuplicatable>
{
    /// <inheritdoc/>
    protected override IDuplicatable Copy(IDuplicatable source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, duplicator);

        return source.DeepClone(duplicator);
    }
}
