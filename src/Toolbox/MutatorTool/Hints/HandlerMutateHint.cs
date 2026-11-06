using System.Security.Cryptography;
using System.Text;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.MutatorTool.Engine;
using Werecodent.CreateAndFake.MutatorTool.Handlers;

namespace Werecodent.CreateAndFake.MutatorTool.Hints;

#pragma warning disable CA1308 // Uri support for .NET 4.8 is lowercase.

/// <summary>Combines and utilizes available handlers for mutations.</summary>
public sealed class HandlerMutateHint : IMutateHint
{
    /// <summary>Handlers to use that haven't already been specified.</summary>
    private static readonly IMutateHandler[] _Creators =
    [
        new StringDictionaryMutateHandler(),
        new NoMutateHandler(typeof(string)),
        new NoMutateHandler(typeof(ECCurve)),
        new NoMutateHandler(typeof(RuntimeMethodHandle)),
        new FactoryMutateHandler<UriBuilder>(
            (instance, mutator) =>
                instance.Host = mutator.Options.Randomizer.Create<string>().ToLowerInvariant()
        ),
        new FactoryMutateHandler<StringBuilder>(
            (instance, mutator) => instance.Append(mutator.Options.Randomizer.Create<string>())
        ),
        new FactoryMutateHandler<StreamWriter>(
            (instance, mutator) =>
            {
                instance.WriteLine(mutator.Options.Randomizer.Create<string>());
                instance.Flush();
            }
        ),
        new FactoryMutateHandler<StreamReader>(
            (instance, mutator) => mutator.Modify(instance.BaseStream)
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
    public MutateHintResult TryToModify(object instance, IMutatorChainer chainer)
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
        return GenericConverter.ExpandName(GetType());
    }
}

#pragma warning restore
