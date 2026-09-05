using System.Reflection;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.MutatorTool.Engine;

namespace Werecodent.CreateAndFake.MutatorTool.Hints;

/// <summary>Handles the mutation of <see cref="ICollection{T}"/>s.</summary>
public sealed class CollectionMutateHint : MutateHint
{
    /// <summary>Handles modifying behavior once generics are specified.</summary>
    private static readonly MethodInfo _Modifier = typeof(CollectionMutateHint).GetMethod(
        nameof(Alter),
        BindingFlags.NonPublic | BindingFlags.Static
    )!;

    /// <inheritdoc/>
    public override int EnginePriority => (int)MutatePriority.CollectionHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes { get; } = [typeof(ICollection<>)];

    /// <inheritdoc/>
    protected override bool Supports(object instance)
    {
        return TypeDescriber.For(instance?.GetType()).Inherits(typeof(ICollection<>));
    }

    /// <inheritdoc/>
    protected override bool Modify(object instance, IMutatorChainer chainer)
    {
        return (bool)
            _Modifier
                .MakeGenericMethod(
                    GenericConverter
                        .FindConcreteType(instance.GetType(), typeof(ICollection<>))
                        .GetGenericArguments()
                )
                .Invoke(null, [instance, chainer])!;
    }

    /// <inheritdoc cref="Modify"/>
    private static bool Alter<T>(ICollection<T> instance, IMutatorChainer chainer)
    {
        bool modified = false;
        if (!instance.IsReadOnly)
        {
            instance.Add(chainer.Options.Randomizer.Create<T>());
            modified = true;
        }

        foreach (T item in chainer.Options.Gen.NextSequence(instance))
        {
            if (modified && chainer.Options.Gen.Next<bool>())
            {
                break;
            }

            modified |= chainer.Modify(item);
        }

        return modified;
    }
}
