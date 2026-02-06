using System.Text;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.MutatorTool.Handlers;

namespace CreateAndFake.MutatorTool.Hints;

/// <inheritdoc/>
public sealed class HandlerMutateHint : IMutateHint
{
    /// <summary>Supported types and the methods used to generate them.</summary>
    private static readonly IMutateHandler[] _Creators =
    [
        new FactoryMutateHandler<StringBuilder>(
            (instance, mutator) => instance.Append(mutator.Options.Randomizer.Create<string>())
        ),
    ];

    /// <summary>All handlers by their supported type.</summary>
    private static readonly IDictionary<Type, IMutateHandler> _MutatorsByType =
        TypeSupporter.GroupBySupportedType(_Creators);

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
}
