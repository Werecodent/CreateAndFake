using System.Collections;
using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.MutatorTool.Handlers;

/// <summary>Handles mutating the individual contents of <see cref="ICollection"/>s.</summary>
internal sealed class CollectionInternalsMutateHandler : IMutateHandler
{
    /// <inheritdoc/>
    public Type? SupportedType => typeof(ICollection);

    /// <inheritdoc/>
    public bool ModifySupported(object instance, IMutatorChainer chainer)
    {
        bool modified = false;

        foreach (
            object item in chainer.Options.Gen.NextSequence(((ICollection)instance).Cast<object>())
        )
        {
            if (modified && chainer.Options.Gen.Next<bool>())
            {
                break;
            }

            modified &= chainer.Modify(item);
        }

        return modified;
    }
}
