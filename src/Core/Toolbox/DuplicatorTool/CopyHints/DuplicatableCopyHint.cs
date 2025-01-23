using CreateAndFake.Design;

namespace CreateAndFake.Toolbox.DuplicatorTool.CopyHints;

/// <summary>Handles cloning <see cref="IDuplicatable"/> instances for <see cref="IDuplicator"/> .</summary>
public sealed class DuplicatableCopyHint : CopyHint<IDuplicatable>
{
    /// <inheritdoc/>
    protected override IDuplicatable Copy(IDuplicatable source, DuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, nameof(source));
        ArgumentGuard.ThrowIfNull(duplicator, nameof(duplicator));

        return source.DeepClone(duplicator.Duplicator);
    }
}
