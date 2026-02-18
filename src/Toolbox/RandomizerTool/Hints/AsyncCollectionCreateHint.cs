using System.Reflection;
using System.Runtime.CompilerServices;
using CreateAndFake.Design;
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

        if (
            type.Inherits(typeof(IAsyncEnumerable<>))
            && (
                type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>)
                || (
                    type.FullName?.Contains(
                        $"{nameof(AsyncCollectionCreateHint)}+<{nameof(GetItemsCancelableAsync)}>"
                    )
                    ?? false
                )
            )
        )
        {
            Type itemType = type.GetGenericArguments().Single();
            object backingData = randomizer.Create(
                typeof(List<>).MakeGenericType(itemType),
                _ => randomizer.Options
            );
            return new(
                GetType()
                    .GetMethod(nameof(GetItemsAsync), BindingFlags.Static | BindingFlags.NonPublic)!
                    .MakeGenericMethod(itemType)
                    .Invoke(null, [backingData])
            );
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <inheritdoc cref="GetItemsCancelableAsync"/>
    private static IAsyncEnumerable<T> GetItemsAsync<T>(List<T> backing)
    {
        return GetItemsCancelableAsync(backing);
    }

    /// <summary>Supplies collection items asynchronously.</summary>
    /// <typeparam name="T">Item <see cref="Type"/> to supply.</typeparam>
    /// <param name="backing">Collection items to supply.</param>
    /// <param name="canceler">Aborts execution if triggered.</param>
    /// <returns>The collection made from <paramref name="backing"/>.</returns>
    private static async IAsyncEnumerable<T> GetItemsCancelableAsync<T>(
        List<T> backing,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        for (int i = 0; i < backing.Count; i++)
        {
            await Task.Delay(0, canceler).ConfigureAwait(false);
            yield return backing[i];
        }
    }
}

#pragma warning restore CA1307
