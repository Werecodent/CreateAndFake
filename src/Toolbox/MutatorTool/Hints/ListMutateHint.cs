using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.Design.Types;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.MutatorTool.Handlers;

namespace CreateAndFake.MutatorTool.Hints;

/// <inheritdoc/>
public sealed class ListMutateHint : MutateHint<IList>
{
    /// <summary>Handles mutating the collection's contents.</summary>
    private static readonly CollectionInternalsMutateHandler _Handler = new();

    /// <inheritdoc/>
    public override int EnginePriority => (int)MutatePriority.ListHint;

    /// <inheritdoc/>
    protected override bool Modify(IList instance, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (instance.IsReadOnly || (instance.Count == 0 && instance.IsFixedSize))
        {
            return _Handler.ModifySupported(instance, chainer);
        }

        Type itemType =
            GenericTypeConverter
                .AsConcreteType(instance.GetType(), typeof(IEnumerable<>))
                ?.GetGenericArguments()[0]
            ?? instance.Cast<object>().Select(d => d?.GetType()).FirstOrDefault(t => t != null)
            ?? typeof(string);

        object newValue = chainer.Options.Randomizer.Create(itemType);

        bool modified = false;
        if (!instance.IsFixedSize && (instance.Count == 0 || chainer.Options.Gen.Next<bool>()))
        {
            modified = instance.Add(newValue) != -1;
        }

        if (!modified && instance.Count > 0)
        {
            instance[chainer.Options.Gen.Next(instance.Count)] = newValue;
            modified = true;
        }

        if (chainer.Options.Gen.Next<bool>())
        {
            modified |= _Handler.ModifySupported(instance, chainer);
        }
        return modified;
    }
}
