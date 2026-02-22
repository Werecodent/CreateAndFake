using System.Collections;
using System.Collections.Immutable;
using CreateAndFake.Design;
using CreateAndFake.Design.Types;
using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.MutatorTool.Hints;

/// <summary>
///     Handles the mutation of collections that don't support <see cref="ICollection{T}"/>s.
/// </summary>
public sealed class ImmutableEnumerableMutateHint : MutateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)MutatePriority.ImmutableEnumerableHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes =>
        [typeof(ImmutableQueue<>), typeof(ImmutableStack<>)];

    /// <inheritdoc/>
    protected override bool Supports(object instance)
    {
        InheritanceTracker inheritance = InheritanceTracker.For(instance?.GetType());

        return inheritance.Inherits(typeof(ImmutableQueue<>))
            || inheritance.Inherits(typeof(ImmutableStack<>));
    }

    /// <inheritdoc/>
    protected override bool Modify(object instance, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);
        bool modified = false;

        foreach (
            object item in chainer.Options.Gen.NextSequence(((IEnumerable)instance).Cast<object>())
        )
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
