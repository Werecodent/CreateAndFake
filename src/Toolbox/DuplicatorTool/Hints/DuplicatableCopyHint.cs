using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning <see cref="IDuplicatable{T}"/> instances for <see cref="IDuplicator"/>.</summary>
public sealed class DuplicatableCopyHint : CopyHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.DuplicatableHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(IDuplicatable<>)];

    /// <inheritdoc/>
    public override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source);

        Type sourceType = source.GetType();
        if (sourceType.Inherits(typeof(IDuplicatable<>)))
        {
            Type cloneType;
            try
            {
                cloneType = typeof(IDuplicatable<>).MakeGenericType(sourceType);
            }
            catch (TypeLoadException)
            {
                return CopyHintResult.None;
            }

            return new(
                cloneType.GetMethod(nameof(IDuplicatable<>.DeepClone))!.Invoke(source, [duplicator])
            );
        }
        else
        {
            return CopyHintResult.None;
        }
    }
}
