using System.Reflection;
using CreateAndFake.Design;

#pragma warning disable CA1307 // Specify StringComparison for clarity: Not available for all versions.

namespace CreateAndFake.RandomizerTool.CreateHints;

/// <summary>Handles randomizing <see cref="IAsyncEnumerable{T}"/> collections for <see cref="IRandomizer"/>.</summary>
public sealed class AsyncCollectionCreateHint : CreateHint
{
    /// <inheritdoc/>
    public override CreateHintResult TryCreate(Type type, RandomizerChainer randomizer)
    {
        ArgumentGuard.ThrowIfNull(randomizer, nameof(randomizer));

        if (
            type.Inherits(typeof(IAsyncEnumerable<>))
            && (
                type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>)
                || (
                    type.FullName?.Contains(
                        $"{nameof(AsyncCollectionCreateHint)}+<{nameof(GetItems)}>"
                    ) ?? false
                )
            )
        )
        {
            Type itemType = type.GetGenericArguments().Single();
            return new(
                GetType()
                    .GetMethod(nameof(GetItems), BindingFlags.Static | BindingFlags.NonPublic)!
                    .MakeGenericMethod(itemType)
                    .Invoke(
                        null,
                        [
                            randomizer.Create(
                                typeof(List<>).MakeGenericType(itemType),
                                randomizer.Options
                            ),
                        ]
                    )
            );
        }
        else
        {
            return CreateHintResult.None;
        }
    }

    /// <summary>Supplies collection items asynchronously.</summary>
    /// <typeparam name="T">Item <c>Type</c> to supply.</typeparam>
    /// <param name="backing">Collection items to supply.</param>
    /// <returns>The collection made from <paramref name="backing"/>.</returns>
    private static async IAsyncEnumerable<T> GetItems<T>(List<T> backing)
    {
        for (int i = 0; i < backing.Count; i++)
        {
            await Task.Delay(0).ConfigureAwait(false);
            yield return backing[i];
        }
    }
}

#pragma warning restore CA1307 // Specify StringComparison for clarity
