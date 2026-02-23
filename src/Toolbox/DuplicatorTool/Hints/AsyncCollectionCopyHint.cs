using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.DuplicatorTool.Engine;

namespace CreateAndFake.DuplicatorTool.Hints;

#pragma warning disable MA0079 // Should not cancel the source while canceling the clone.

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
            return new(CopyAsync((dynamic)source, duplicator));
        }
        else
        {
            return CopyHintResult.None;
        }
    }

    /// <typeparam name="T">Item type being copied.</typeparam>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>Iteration of cloned <paramref name="source"/> values.</returns>
    /// <inheritdoc cref="TryCopy"/>
    private static async IAsyncEnumerable<T?> CopyAsync<T>(
        IAsyncEnumerable<T> source,
        IDuplicatorChainer duplicator,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        int index = 0;
        await foreach (T item in source.ConfigureAwait(false))
        {
            ArgumentGuard.ThrowUponIterationLimit(
                index++,
                duplicator.Options.Valuer.Options.IterationLimit
            );
            canceler.ThrowIfCancellationRequested();
            yield return duplicator.Copy(item);
        }
    }
}

#pragma warning restore MA0079
