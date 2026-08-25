using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning <see cref="IAsyncEnumerable{T}"/> collections for <see cref="IDuplicator"/> .</summary>
public sealed class AsyncDesignCopyHint : CopyHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.AsyncDesignHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(AsyncList<>)];

    /// <inheritdoc/>
    public override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, duplicator);

        Type type = source.GetType();
        Type? asGeneric = GenericConverter.AsGenericBase(type);

        if (asGeneric == typeof(AsyncList<>))
        {
            return new(
                Activator.CreateInstance(
                    type,
                    duplicator.Copy(((dynamic)source).Content),
                    int.MaxValue
                )
            );
        }
        else
        {
            return CopyHintResult.None;
        }
    }
}
