using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ValuerTool.Engine;

#pragma warning disable MA0042 // Using sync behavior for async versions.

/// <summary>Handles comparing specific types for <see cref="IValuer"/>.</summary>
public abstract class CompareHint : IToolHint
{
    /// <inheritdoc/>
    public abstract int EnginePriority { get; }

    /// <inheritdoc/>
    public virtual IEnumerable<Type> SupportedTypes { get; } = [];

    /// <summary>
    ///     Tries to find the differences between <paramref name="expected"/> and <paramref name="actual"/>.
    /// </summary>
    /// <param name="expected">Object to compare with <paramref name="actual"/>.</param>
    /// <param name="actual">Potentially different object to compare against <paramref name="expected"/>.</param>
    /// <param name="valuer">Handles comparing child values.</param>
    /// <returns>Possible result.</returns>
    public DifferenceHintResult TryCompare(object? expected, object? actual, IValuerChainer valuer)
    {
        ArgumentGuard.ThrowIfNull(valuer);

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
        ArgumentGuard.ThrowIfNull(valuer);

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
    /// <param name="canceler">Aborts execution if triggered.</param>
    private async IAsyncEnumerable<Difference> HandleAsyncCompare(
        object? expected,
        object? actual,
        IValuerChainer valuer,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        ArgumentGuard.ThrowIfNull(canceler);

        await foreach (
            Difference diff in CompareAsync(expected, actual, valuer, canceler)
                .ConfigureAwait(false)
        )
        {
            yield return diff;
        }
        canceler.ThrowIfCancellationRequested();

        if (
            valuer.Options.IncludeValueHashInComparison
            && !ReferenceEquals(expected, actual)
            && expected is not null
            && actual is not null
        )
        {
            int expectedHash = await GetHashCodeAsync(expected, valuer, canceler)
                .ConfigureAwait(false);
            int actualHash = await GetHashCodeAsync(actual, valuer, canceler).ConfigureAwait(false);

            if (expectedHash != actualHash)
            {
                yield return new Difference(
                    "(ValueHash)",
                    new Difference(expectedHash, actualHash)
                );
            }
        }
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
    /// <param name="canceler">Aborts execution if triggered.</param>
    public HashCodeHintAsyncResult TryAsyncGetHashCode(
        object? item,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        if (Supports(item, item, valuer))
        {
            return new(GetHashCodeAsync(item, valuer, canceler));
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
    /// <param name="canceler">Aborts execution if triggered.</param>
    protected virtual IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        return AsyncEnumHelper.CreateFrom(Compare(expected, actual, valuer));
    }

    /// <summary>Computes an identifying hash code for <paramref name="item"/> based upon value.</summary>
    /// <returns>The value computed hash code for <paramref name="item"/>.</returns>
    /// <inheritdoc cref="TryGetHashCode"/>
    protected abstract int GetHashCode(object? item, IValuerChainer valuer);

    /// <inheritdoc cref="GetHashCode"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    protected virtual Task<int> GetHashCodeAsync(
        object? item,
        IValuerChainer valuer,
        CancellationToken canceler
    )
    {
        return Task.FromResult(GetHashCode(item, valuer));
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}

#pragma warning restore MA0042 // Using sync behavior for async versions.
