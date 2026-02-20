using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.RandomizerTool.Hints;

#pragma warning disable CA1307 // Not available for all versions.

/// <summary>Handles randomizing <see cref="IAsyncEnumerable{T}"/> collections for <see cref="IRandomizer"/>.</summary>
public sealed class AsyncCollectionCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CreatePriority.AsyncCollectionHint;

    /// <inheritdoc/>
    public override IEnumerable<Type> SupportedTypes => [typeof(IAsyncEnumerable<>)];

    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, IRandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer);

        Type? genericType = TypeDescriber.AsConcreteType(type, typeof(IAsyncEnumerable<>));
        if (genericType != null)
        {
            Type itemType = genericType.GetGenericArguments().Single();
            object backingData = randomizer.Create(
                typeof(List<>).MakeGenericType(itemType),
                _ => randomizer.Options
            );
            return new(GetItemsAsync((dynamic)backingData));
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <summary>Supplies collection items asynchronously.</summary>
    /// <typeparam name="T">Item <see cref="Type"/> to supply.</typeparam>
    /// <param name="backing">Collection items to supply.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>The collection made from <paramref name="backing"/>.</returns>
    private static async IAsyncEnumerable<T> GetItemsAsync<T>(
        List<T> backing,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        for (int i = 0; i < backing.Count; i++)
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            canceler.ThrowIfCancellationRequested();
            yield return backing[i];
        }
    }
}

#pragma warning restore CA1307
