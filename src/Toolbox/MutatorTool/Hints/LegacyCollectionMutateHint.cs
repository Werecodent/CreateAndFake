using System.Collections;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.MutatorTool.Engine;
using Werecodent.CreateAndFake.MutatorTool.Handlers;

namespace Werecodent.CreateAndFake.MutatorTool.Hints;

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
