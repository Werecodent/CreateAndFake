using System.Text;
using CreateAndFake.Design;
using CreateAndFake.Design.Types;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.MutatorTool.Handlers;

namespace CreateAndFake.MutatorTool.Hints;

/// <summary>Combines and utilizes available handlers for mutations.</summary>
public sealed class HandlerMutateHint : IMutateHint
{
    /// <summary>Handlers to use that haven't already been specified.</summary>
    private static readonly IMutateHandler[] _Creators =
    [
        new StringDictionaryMutateHandler(),
        new NoMutateHandler(typeof(string)),
        new FactoryMutateHandler<StringBuilder>(
            (instance, mutator) => instance.Append(mutator.Options.Randomizer.Create<string>())
        ),
    ];

    /// <summary>All handlers by their supported type.</summary>
    private static readonly IDictionary<Type, IMutateHandler> _MutatorsByType =
        TypeSupporter.GroupBySupportedType(_Creators.Concat(ReflectionMutateHandlers.Handlers));

    /// <inheritdoc/>
    public int EnginePriority => (int)MutatePriority.HandlerHint;

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes => _MutatorsByType.Keys;

    /// <inheritdoc/>
    public MutateHintResult TryModifying(object instance, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(instance, chainer);

        if (_MutatorsByType.TryGetValue(instance.GetType(), out IMutateHandler? handler))
        {
            return new(handler.ModifySupported(instance, chainer));
        }
        else
        {
            return MutateHintResult.None;
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
