using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;

namespace CreateAndFake.ValuerTool.CompareHints;

/// <summary>Handles comparing <see cref="IAsyncEnumerable{T}"/> collections for <see cref="IValuer"/>.</summary>
public sealed class AsyncEnumerableCompareHint : CompareHint
{
    /// <inheritdoc/>
    protected override bool Supports(object? expected, object? actual, ValuerChainer valuer)
    {
        return (expected?.GetType()).Inherits(typeof(IAsyncEnumerable<>))
            && (actual?.GetType()).Inherits(typeof(IAsyncEnumerable<>));
    }

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        object? expected, object? actual, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(expected, nameof(expected));
        ArgumentGuard.ThrowIfNull(actual, nameof(actual));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return Task.Run(() => (Task<IEnumerable<Difference>>)GetType()
            .GetMethod(nameof(CompareAsync), BindingFlags.Static | BindingFlags.NonPublic)!
            .MakeGenericMethod(expected.GetType().GetGenericArguments().Single())
            .Invoke(null, [expected, actual, valuer])!).Result;
    }

    /// <inheritdoc cref="Compare"/>
    /// <typeparam name="T">Item <c>Type</c> being compared.</typeparam>
    private static async Task<IEnumerable<Difference>> CompareAsync<T>(
        IAsyncEnumerable<T> expected, IAsyncEnumerable<T> actual, ValuerChainer valuer)
    {
        List<Difference> differences = [];

        if (valuer.Options.CheckCollectionType && expected.GetType() != actual.GetType())
        {
            differences.Add(new Difference(expected.GetType(), actual.GetType()));
        }

        IAsyncEnumerator<T> expectedEnumerator = expected.GetAsyncEnumerator();
        IAsyncEnumerator<T> actualEnumerator = actual.GetAsyncEnumerator();
        await using (expectedEnumerator.ConfigureAwait(false))
        await using (actualEnumerator.ConfigureAwait(false))
        {
            int index = 0;
            while (await expectedEnumerator.MoveNextAsync().ConfigureAwait(false))
            {
                if (await actualEnumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    differences.AddRange(valuer
                        .Compare(expectedEnumerator.Current, actualEnumerator.Current)
                        .Select(diff => new Difference(index, diff)));
                }
                else
                {
                    differences.Add(new Difference(index, new Difference(expectedEnumerator.Current, "'outofbounds'")));
                }
                index++;
            }
            while (await actualEnumerator.MoveNextAsync().ConfigureAwait(false))
            {
                differences.Add(new Difference(index++, new Difference("'outofbounds'", actualEnumerator.Current)));
            }
        }

        return differences;
    }

    /// <inheritdoc/>
    protected override int GetHashCode(object? item, ValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(item, nameof(item));
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        return Task.Run(() => (Task<int>)GetType()
            .GetMethod(nameof(GetHashCodeAsync), BindingFlags.Static | BindingFlags.NonPublic)!
            .MakeGenericMethod(item.GetType().GetGenericArguments().Single())
            .Invoke(null, [item, valuer])!).Result;
    }

    /// <inheritdoc cref="GetHashCode"/>
    /// <typeparam name="T">Item <c>Type</c> being compared.</typeparam>
    private static async Task<int> GetHashCodeAsync<T>(IAsyncEnumerable<T> item, ValuerChainer valuer)
    {
        int hash = ValueComparer.BaseHash;
        await foreach (T current in item.ConfigureAwait(false))
        {
            hash = hash * ValueComparer.HashMultiplier + valuer.GetHashCode(current);
        }
        return hash;
    }
}
