using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning <see cref="IDeepCloneable{T}"/> instances for <see cref="IDuplicator"/>.</summary>
public sealed class DeepCloneableCopyHint : CopyHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.DeepCloneableHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(IDeepCloneable<>)];

    /// <inheritdoc/>
    public override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source);

        Type sourceType = source.GetType();
        if (sourceType.Inherits(typeof(IDeepCloneable<>)))
        {
            Type cloneType;
            try
            {
                cloneType = typeof(IDeepCloneable<>).MakeGenericType(sourceType);
            }
            catch (TypeLoadException)
            {
                return CopyHintResult.None;
            }

            return new(cloneType.GetMethod(nameof(IDeepCloneable<>.DeepClone))!.Invoke(source, []));
        }
        else
        {
            return CopyHintResult.None;
        }
    }
}
