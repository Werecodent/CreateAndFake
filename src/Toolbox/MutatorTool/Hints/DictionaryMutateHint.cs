using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.Design.Types;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.MutatorTool.Handlers;

namespace CreateAndFake.MutatorTool.Hints;

/// <inheritdoc/>
public sealed class DictionaryMutateHint : MutateHint<IDictionary>
{
    /// <summary>Handles mutating the collection's contents.</summary>
    private static readonly CollectionInternalsMutateHandler _Handler = new();

    /// <inheritdoc/>
    public override int EnginePriority => (int)MutatePriority.DictionaryHint;

    /// <inheritdoc/>
    protected override bool Modify(IDictionary instance, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (instance.IsReadOnly || (instance.Count == 0 && instance.IsFixedSize))
        {
            return _Handler.ModifySupported(instance, chainer);
        }

        Type? pairType = GenericConverter.AsConcreteType(
            instance.GetType(),
            typeof(IDictionary<,>)
        );

        Type keyType =
            pairType?.GetGenericArguments()[0]
            ?? instance.Keys.Cast<object>().Select(d => d.GetType()).FirstOrDefault()
            ?? typeof(string);

        Type valueType =
            pairType?.GetGenericArguments()[1]
            ?? instance
                .Values.Cast<object>()
                .Select(d => d?.GetType())
                .FirstOrDefault(t => t != null)
            ?? typeof(string);

        object key =
            instance.IsFixedSize || (instance.Count > 0 && chainer.Options.Gen.Next<bool>())
                ? chainer.Options.Gen.NextItem(instance.Keys.Cast<object>())
                : chainer.VariantOf(keyType, instance.Keys.Cast<object>());

        instance[key] = chainer.Options.Randomizer.Create(valueType);

        if (chainer.Options.Gen.Next<bool>())
        {
            _ = _Handler.ModifySupported(instance, chainer);
        }
        return true;
    }
}
