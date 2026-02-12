using System.Collections;
using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.MutatorTool.Hints;

namespace CreateAndFake.MutatorTool.Handlers;

/// <summary>Handles mutating the individual contents of <see cref="ICollection"/>s.</summary>
/// <remarks>Not for <see cref="HandlerMutateHint"/>; intended to help other hints.</remarks>
internal sealed class CollectionInternalsMutateHandler : IMutateHandler
{
    /// <inheritdoc/>
    public Type? SupportedType => null;

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

            modified |= chainer.Modify(item);
        }

        return modified;
    }
}
