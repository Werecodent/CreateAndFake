using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.MutatorTool.Handlers;

namespace CreateAndFake.MutatorTool.Hints;

/// <inheritdoc/>
public class LegacyCollectionMutateHint : MutateHint<ICollection>
{
    /// <summary>Handles mutating the collection's contents.</summary>
    private static readonly CollectionInternalsMutateHandler _Handler = new();

    /// <inheritdoc/>
    public override int EnginePriority => (int)MutatePriority.LegacyCollectionHint;

    /// <inheritdoc/>
    protected override bool Modify(ICollection instance, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);
        return _Handler.ModifySupported(instance, chainer);
    }
}
