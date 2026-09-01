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
                    duplicator.Copy(((dynamic)source).Content, null),
                    int.MaxValue
                )
            );
        }
        else if (asGeneric == typeof(AsyncHashSet<>))
        {
            return new(CopyContentsAsync((dynamic)source, duplicator));
        }
        else
        {
            return CopyHintResult.None;
        }
    }

    /// <typeparam name="T">Item type being copied.</typeparam>
    /// <returns>Iteration of cloned <paramref name="source"/> values.</returns>
    /// <inheritdoc cref="TryCopy"/>
    private static AsyncHashSet<T> CopyContentsAsync<T>(
        AsyncHashSet<T> source,
        IDuplicatorChainer duplicator
    )
    {
        return AsyncHashSet<T>.CreateFromAsync(
            duplicator.Copy(source.ByHashesAsync(CancellationToken.None)),
            source.Comparer,
            duplicator.Options.Valuer.Options.IterationLimit,
            CancellationToken.None
        );
    }
}
