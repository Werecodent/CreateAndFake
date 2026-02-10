using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.MutatorTool.Handlers;

namespace CreateAndFake.MutatorTool.Hints;

/// <inheritdoc/>
public sealed class LegacyListMutateHint : MutateHint<IList>
{
    /// <summary>Handles mutating the collection's contents.</summary>
    private static readonly CollectionInternalsMutateHandler _Handler = new();

    /// <inheritdoc/>
    public override int EnginePriority => (int)MutatePriority.LegacyListHint;

    /// <inheritdoc/>
    protected override bool Modify(IList instance, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (instance.IsReadOnly || (instance.Count == 0 && instance.IsFixedSize))
        {
            return _Handler.ModifySupported(instance, chainer);
        }

        string newValue = chainer.Options.Randomizer.Create<string>();
        if (
            instance.IsFixedSize
            || chainer.Options.Gen.Next<bool>()
            || instance.Add(newValue) == -1
        )
        {
            instance[chainer.Options.Gen.Next(instance.Count)] = newValue;
        }

        if (chainer.Options.Gen.Next<bool>())
        {
            _ = _Handler.ModifySupported(instance, chainer);
        }
        return true;
    }
}
