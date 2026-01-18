using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.ValuerTool.Engine;

/// <summary>Provides a callback into <see cref="IValuer"/> to create child values.</summary>
public sealed class ValuerChainer
    : ToolChainer<ValuerChainer, IValuerEngine, ValuerOptions, CompareHint>,
        IValuerChainer
{
    /// <summary>History of hashes to match up references.</summary>
    private readonly Dictionary<int, object?> _hashHistory;

    /// <summary>History of comparisons to match up references.</summary>
    private readonly HashSet<(int, int)> _compareHistory;

    /// <inheritdoc/>
    public ValuerChainer(ValuerOptions options, IValuerEngine engine)
        : base(options, engine)
    {
        _hashHistory = [];
        _compareHistory = [];
    }

    /// <inheritdoc/>
    private ValuerChainer(ValuerOptions options, ValuerChainer prevChainer)
        : base(options, prevChainer)
    {
        _hashHistory = prevChainer._hashHistory;
        _compareHistory = prevChainer._compareHistory;
    }

    /// <inheritdoc/>
    protected override ValuerChainer CreateSubChainer(ValuerOptions options)
    {
        return new ValuerChainer(options, this);
    }

    /// <summary>If <paramref name="item"/> can be tracked in history.</summary>
    /// <param name="item">Item to check.</param>
    /// <returns><see langword="true"/> if tracking <paramref name="item"/> is possible, <see langword="false"/> otherwise.</returns>
    private static bool CanTrack([NotNullWhen(true)] object? item)
    {
        return !(item == null || item is IFaked || item.GetType().IsValueType);
    }

    /// <inheritdoc/>
    public IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        ValuerMod? optionConfiguration = null
    )
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        if (CanTrack(expected) && CanTrack(actual))
        {
            (int, int) refHash = (
                RuntimeHelpers.GetHashCode(expected),
                RuntimeHelpers.GetHashCode(actual)
            );

            if (_compareHistory.Add(refHash))
            {
                // Yield return is required here to prevent infinite loops.
                foreach (
                    Difference diff in Engine.Compare(
                        expected,
                        actual,
                        GetSubChainer(optionConfiguration)
                    )
                )
                {
                    yield return diff;
                }
            }
        }
        else
        {
            foreach (
                Difference diff in Engine.Compare(
                    expected,
                    actual,
                    GetSubChainer(optionConfiguration)
                )
            )
            {
                yield return diff;
            }
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        ValuerMod? optionConfiguration = null
    )
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        if (CanTrack(expected) && CanTrack(actual))
        {
            (int, int) refHash = (
                RuntimeHelpers.GetHashCode(expected),
                RuntimeHelpers.GetHashCode(actual)
            );

            if (_compareHistory.Add(refHash))
            {
                try
                {
                    // Await & yield return is required here to prevent infinite loops.
                    await foreach (
                        Difference diff in Engine
                            .CompareAsync(expected, actual, GetSubChainer(optionConfiguration))
                            .ConfigureAwait(false)
                    )
                    {
                        yield return diff;
                    }
                }
                finally
                {
                    _ = _compareHistory.Remove(refHash);
                }
            }
        }
        else
        {
            await foreach (
                Difference diff in Engine
                    .CompareAsync(expected, actual, GetSubChainer(optionConfiguration))
                    .ConfigureAwait(false)
            )
            {
                yield return diff;
            }
        }
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item)
    {
        return GetHashCode(item, null);
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item, ValuerMod? optionConfiguration)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        if (!CanTrack(item))
        {
            return Engine.GetHashCode(item, GetSubChainer(optionConfiguration));
        }

        int refHash = RuntimeHelpers.GetHashCode(item);
        if (_hashHistory.TryGetValue(refHash, out object? stored) && ReferenceEquals(item, stored))
        {
            return 0;
        }

        _hashHistory[refHash] = item;
        try
        {
            return Engine.GetHashCode(item, GetSubChainer(optionConfiguration));
        }
        finally
        {
            _ = _hashHistory.Remove(refHash);
        }
    }

    /// <inheritdoc/>
    public async Task<int> GetHashCodeAsync(
        object? item,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        if (!CanTrack(item))
        {
            return await Engine
                .GetHashCodeAsync(item, GetSubChainer(optionConfiguration), canceler)
                .ConfigureAwait(false);
        }

        int refHash = RuntimeHelpers.GetHashCode(item);
        if (_hashHistory.TryGetValue(refHash, out object? stored) && ReferenceEquals(item, stored))
        {
            return 0;
        }

        _hashHistory[refHash] = item;
        try
        {
            // Await is required here to prevent infinite loops.
            return await Engine
                .GetHashCodeAsync(item, GetSubChainer(optionConfiguration), canceler)
                .ConfigureAwait(false);
        }
        finally
        {
            _ = _hashHistory.Remove(refHash);
        }
    }

    /// <inheritdoc/>
    public new bool Equals(object? x, object? y)
    {
        return !Compare(x, y).Any();
    }

    /// <inheritdoc/>
    public bool Equals(object? x, object? y, ValuerMod? optionConfiguration)
    {
        return !Compare(x, y, optionConfiguration).Any();
    }

    /// <inheritdoc/>
    public async Task<bool> EqualsAsync(
        object? x,
        object? y,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        return !await AsyncEnumHelper
            .HasAnyAsync(CompareAsync(x, y, optionConfiguration), canceler)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public IValuer WithOptions(ValuerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new ValuerChainer(optionConfiguration.Invoke(Options), this);
    }
}
