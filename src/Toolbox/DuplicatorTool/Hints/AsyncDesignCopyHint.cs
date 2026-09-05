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
            return new(CopyListAsync((dynamic)source, duplicator));
        }
        else if (asGeneric == typeof(AsyncHashSet<>))
        {
            return new(CopySetAsync((dynamic)source, duplicator));
        }
        else
        {
            return CopyHintResult.None;
        }
    }

    /// <typeparam name="T">Item type being copied.</typeparam>
    /// <returns>Iteration of cloned <paramref name="source"/> values.</returns>
    /// <inheritdoc cref="TryCopy"/>
    private static AsyncList<T> CopyListAsync<T>(AsyncList<T> source, IDuplicatorChainer duplicator)
    {
        // Beware that 'duplicator.Copy' does not work in dynamic context for legacy .NET.
        return new AsyncList<T>(duplicator.Copy(source.Content), int.MaxValue);
    }

    /// <inheritdoc cref="CopyListAsync"/>
    private static AsyncHashSet<T> CopySetAsync<T>(
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
