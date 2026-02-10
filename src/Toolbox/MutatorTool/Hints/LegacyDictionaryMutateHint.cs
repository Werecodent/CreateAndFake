using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.MutatorTool.Handlers;

namespace CreateAndFake.MutatorTool.Hints;

/// <inheritdoc/>
public sealed class LegacyDictionaryMutateHint : MutateHint<IDictionary>
{
    /// <summary>Handles mutating the collection's contents.</summary>
    private static readonly CollectionInternalsMutateHandler _Handler = new();

    /// <inheritdoc/>
    public override int EnginePriority => (int)MutatePriority.LegacyDictionaryHint;

    /// <inheritdoc/>
    protected override bool Modify(IDictionary instance, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (instance.IsReadOnly || (instance.Count == 0 && instance.IsFixedSize))
        {
            return _Handler.ModifySupported(instance, chainer);
        }

        object key =
            instance.IsFixedSize || chainer.Options.Gen.Next<bool>()
                ? chainer.Options.Gen.NextItem(instance.Keys.Cast<object>())
                : chainer.VariantOf(typeof(string), instance.Keys.Cast<object>());

        instance[key] = chainer.Options.Randomizer.Create<string>();

        if (chainer.Options.Gen.Next<bool>())
        {
            _ = _Handler.ModifySupported(instance, chainer);
        }
        return true;
    }
}
