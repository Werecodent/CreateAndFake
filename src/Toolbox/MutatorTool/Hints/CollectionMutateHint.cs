using System.Reflection;
using CreateAndFake.Design.Content;
using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.MutatorTool.Hints;

/// <inheritdoc/>
public sealed class CollectionMutateHint : MutateHint
{
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
        return InheritanceTracker.For(instance.GetType()).Inherits(typeof(ICollection<>));
    }

    /// <inheritdoc/>
    protected override bool Modify(object instance, IMutatorChainer chainer)
    {
        return (bool)
            _Modifier
                .MakeGenericMethod(
                    TypeDescriber
                        .FindConcreteInterface(instance.GetType(), typeof(ICollection<>))
                        .GetGenericArguments()
                )
                .Invoke(null, [instance, chainer])!;
    }

    /// <inheritdoc cref="Modify"/>
    private static bool Alter<T>(ICollection<T> values, IMutatorChainer chainer)
    {
        if (values.IsReadOnly)
        {
            return false;
        }
        else
        {
            values.Add(chainer.Options.Randomizer.Create<T>());
            return true;
        }
    }
}
