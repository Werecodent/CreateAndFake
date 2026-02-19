using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ValuerTool.Engine;

/// <summary>Provides a callback into <see cref="IValuer"/> to create child values.</summary>
public sealed class ValuerChainer
    : ToolChainer<ValuerChainer, IValuerEngine, ValuerOptions, ICompareHint>,
        IValuerChainer
{
    /// <summary>History of hashes to match up references.</summary>
    private readonly ISet<object?> _hashHistory;

    /// <summary>History of comparisons to match up references.</summary>
    private readonly HashSet<(int, int)> _compareHistory;

    /// <inheritdoc/>
    public ValuerChainer(ValuerOptions options, IValuerEngine engine)
        : base(options, engine)
    {
        _hashHistory = new HashSet<object?>(ReferenceComparer.Use);
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
    protected override ValuerChainer CreateSubChainer(ValuerOptions subOptions)
    {
        return new ValuerChainer(subOptions, this);
    }

    /// <summary>If <paramref name="item"/> can be tracked in history.</summary>
    /// <param name="item">Item to check.</param>
    /// <returns><see langword="true"/> if tracking <paramref name="item"/> is possible, <see langword="false"/> otherwise.</returns>
    private static bool CanTrack([NotNullWhen(true)] object? item)
    {
        return item?.GetType().IsValueType == false;
    }

    /// <inheritdoc/>
    public IEnumerable<Difference> Compare(
        object? expected,
        object? actual,
        ValuerMod? optionConfiguration = null
    )
    {
        (int, int)? refHash =
            (CanTrack(expected) && CanTrack(actual))
                ? (RuntimeHelpers.GetHashCode(expected), RuntimeHelpers.GetHashCode(actual))
                : null;

        if (refHash == null || _compareHistory.Add(refHash.Value))
        {
            try
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
            finally
            {
                if (refHash != null)
                {
                    _ = _compareHistory.Remove(refHash.Value);
                }
            }
        }
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        return HandleCompareAsync(expected, actual, optionConfiguration, canceler);
    }

    /// <inheritdoc/>
    private async IAsyncEnumerable<Difference> HandleCompareAsync(
        object? expected,
        object? actual,
        ValuerMod? optionConfiguration,
        [EnumeratorCancellation] CancellationToken canceler = default
    )
    {
        (int, int)? refHash =
            (CanTrack(expected) && CanTrack(actual))
                ? (RuntimeHelpers.GetHashCode(expected), RuntimeHelpers.GetHashCode(actual))
                : null;

        if (refHash == null || _compareHistory.Add(refHash.Value))
        {
            try
            {
                // Await & yield return is required here to prevent infinite loops.
                await foreach (
                    Difference diff in Engine
                        .CompareAsync(
                            expected,
                            actual,
                            GetSubChainer(optionConfiguration),
                            canceler
                        )
                        .ConfigureAwait(false)
                )
                {
                    yield return diff;
                }
            }
            finally
            {
                if (refHash != null)
                {
                    _ = _compareHistory.Remove(refHash.Value);
                }
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
        if (!CanTrack(item))
        {
            return Engine.GetHashCode(item, GetSubChainer(optionConfiguration));
        }

        if (_hashHistory.Add(item))
        {
            try
            {
                return Engine.GetHashCode(item, GetSubChainer(optionConfiguration));
            }
            finally
            {
                _ = _hashHistory.Remove(item);
            }
        }
        else
        {
            return 0;
        }
    }

    /// <inheritdoc/>
    public async Task<int> GetHashCodeAsync(
        object? item,
        CancellationToken canceler,
        ValuerMod? optionConfiguration = null
    )
    {
        if (!CanTrack(item))
        {
            return await Engine
                .GetHashCodeAsync(item, GetSubChainer(optionConfiguration), canceler)
                .ConfigureAwait(false);
        }

        if (_hashHistory.Add(item))
        {
            try
            {
                // Await is required here to prevent infinite loops.
                return await Engine
                    .GetHashCodeAsync(item, GetSubChainer(optionConfiguration), canceler)
                    .ConfigureAwait(false);
            }
            finally
            {
                _ = _hashHistory.Remove(item);
            }
        }
        else
        {
            return 0;
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
            .HasAnyAsync(CompareAsync(x, y, canceler, optionConfiguration), canceler)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public IValuer WithOptions(ValuerMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new ValuerChainer(optionConfiguration.Invoke(Options), this);
    }
}
