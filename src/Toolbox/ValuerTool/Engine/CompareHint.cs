using CreateAndFake.Design;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ValuerTool.Engine;

#pragma warning disable MA0042 // Using sync behavior for async versions.

/// <summary>Handles comparing specific types for <see cref="IValuer"/>.</summary>
public abstract class CompareHint : IToolHint
{
    /// <summary>
    ///     Tries to find the differences between <paramref name="expected"/> and <paramref name="actual"/>.
    /// </summary>
    /// <param name="expected">Object to compare with <paramref name="actual"/>.</param>
    /// <param name="actual">Potentially different object to compare against <paramref name="expected"/>.</param>
    /// <param name="valuer">Handles comparing child values.</param>
    /// <returns>Possible result.</returns>
    public DifferenceHintResult TryCompare(object? expected, object? actual, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        if (Supports(expected, actual, valuer))
        {
            IEnumerable<Difference> results = Compare(expected, actual, valuer);
            if (
                valuer.Options.IncludeValueHashInComparison
                && !ReferenceEquals(expected, actual)
                && expected is not null
                && actual is not null
            )
            {
                int expectedHash = GetHashCode(expected, valuer);
                int actualHash = GetHashCode(actual, valuer);

                if (expectedHash != actualHash)
                {
                    results = results.Append(
                        new Difference("(ValueHash)", new Difference(expectedHash, actualHash))
                    );
                }
            }
            return new(results);
        }
        else
        {
            return DifferenceHintResult.None;
        }
    }

    /// <inheritdoc cref="TryCompare"/>
    public DifferenceHintAsyncResult TryAsyncCompare(
        object? expected,
        object? actual,
        IValuerChainer valuer
    )
    {
        ArgumentGuard.ThrowIfNull(valuer, nameof(valuer));

        if (Supports(expected, actual, valuer))
        {
            return new(HandleAsyncCompare(expected, actual, valuer));
        }
        else
        {
            return DifferenceHintAsyncResult.None;
        }
    }

    /// <inheritdoc cref="TryAsyncCompare"/>
    private async Task<IEnumerable<Difference>> HandleAsyncCompare(
        object? expected,
        object? actual,
        IValuerChainer valuer
    )
    {
        IEnumerable<Difference> results = await CompareAsync(expected, actual, valuer)
            .ConfigureAwait(false);

        if (
            valuer.Options.IncludeValueHashInComparison
            && !ReferenceEquals(expected, actual)
            && expected is not null
            && actual is not null
        )
        {
            int expectedHash = await GetHashCodeAsync(expected, valuer).ConfigureAwait(false);
            int actualHash = await GetHashCodeAsync(actual, valuer).ConfigureAwait(false);

            if (expectedHash != actualHash)
            {
                results = results.Append(
                    new Difference("(ValueHash)", new Difference(expectedHash, actualHash))
                );
            }
        }
        return results;
    }

    /// <summary>Tries to compute an identifying hash code for <paramref name="item"/> based upon value.</summary>
    /// <param name="item">Object to generate a hash code for.</param>
    /// <param name="valuer">Handles hashing behavior for child values.</param>
    /// <returns>Possible result.</returns>
    public HashCodeHintResult TryGetHashCode(object? item, IValuerChainer valuer)
    {
        if (Supports(item, item, valuer))
        {
            return new(GetHashCode(item, valuer));
        }
        else
        {
            return HashCodeHintResult.None;
        }
    }

    /// <inheritdoc cref="TryGetHashCode"/>
    public HashCodeHintAsyncResult TryAsyncGetHashCode(object? item, IValuerChainer valuer)
    {
        if (Supports(item, item, valuer))
        {
            return new(GetHashCodeAsync(item, valuer));
        }
        else
        {
            return HashCodeHintAsyncResult.None;
        }
    }

    /// <summary>
    ///     Determines if <paramref name="expected"/> or <paramref name="actual"/> are supported by the hint.
    /// </summary>
    /// <returns><see langword="true"/> if the objects can be compared, <see langword="false"/> otherwise.</returns>
    /// <inheritdoc cref="TryCompare"/>
    protected abstract bool Supports(object? expected, object? actual, IValuerChainer valuer);

    /// <summary>Finds the differences between <paramref name="expected"/> and <paramref name="actual"/>.</summary>
    /// <returns>The found differences between <paramref name="expected"/> and <paramref name="actual"/>.</returns>
    /// <inheritdoc cref="TryCompare"/>
    protected abstract IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        IValuerChainer valuer
    );

    /// <inheritdoc cref="Compare"/>
    protected virtual Task<IEnumerable<Difference>> CompareAsync(
        object? expected,
        object? actual,
        IValuerChainer valuer
    )
    {
        return Task.FromResult(Compare(expected, actual, valuer));
    }

    /// <summary>Computes an identifying hash code for <paramref name="item"/> based upon value.</summary>
    /// <returns>The value computed hash code for <paramref name="item"/>.</returns>
    /// <inheritdoc cref="TryGetHashCode"/>
    protected abstract int GetHashCode(object? item, IValuerChainer valuer);

    /// <inheritdoc cref="GetHashCode"/>
    protected virtual Task<int> GetHashCodeAsync(object? item, IValuerChainer valuer)
    {
        return Task.FromResult(GetHashCode(item, valuer));
    }
}

#pragma warning restore MA0042 // Using sync behavior for async versions.
