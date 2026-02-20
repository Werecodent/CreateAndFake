using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Hints;

/// <summary>Handles cloning <see cref="IAsyncEnumerable{T}"/> collections for <see cref="IDuplicator"/> .</summary>
public sealed class AsyncCollectionCopyHint : CopyHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.AsyncCollectionHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(IAsyncEnumerable<>)];

    /// <inheritdoc/>
    public override CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source, duplicator);

        if (source.GetType().Inherits(typeof(IAsyncEnumerable<>)))
        {
            return new(
                typeof(AsyncCollectionCopyHint)
                    .GetMethod(nameof(CopyAsync), BindingFlags.Static | BindingFlags.NonPublic)!
                    .MakeGenericMethod(source.GetType().GetGenericArguments().Single())
                    .Invoke(null, [source, duplicator])
            );
        }
        else
        {
            return CopyHintResult.None;
        }
    }

    /// <summary>Deep clones <paramref name="source"/>.</summary>
    /// <typeparam name="T">Item type being copied.</typeparam>
    /// <param name="source">Object to clone.</param>
    /// <param name="duplicator">Handles callback behavior for child values.</param>
    /// <returns>Clone of <paramref name="source"/>.</returns>
    private static async IAsyncEnumerable<T?> CopyAsync<T>(
        IAsyncEnumerable<T> source,
        IDuplicatorChainer duplicator
    )
    {
        await foreach (T item in source.ConfigureAwait(false))
        {
            yield return duplicator.Copy(item);
        }
    }
}
