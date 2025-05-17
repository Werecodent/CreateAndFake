using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.ValuerTool.Engine;

/// <summary>Provides a callback into <see cref="IValuer"/> to create child values.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <param name="engine"><inheritdoc cref="_engine" path="/summary"/></param>
public sealed class ValuerChainer(ValuerOptions options, ValuerEngine engine) : IValuer
{
    /// <summary>History of hashes to match up references.</summary>
    private readonly Dictionary<int, object?> _hashHistory = [];

    /// <summary>History of comparisons to match up references.</summary>
    private readonly HashSet<(int, int)> _compareHistory = [];

    /// <inheritdoc cref="ValuerOptions"/>
    public ValuerOptions Options { get; } = options;

    /// <summary>Callback mechanism..</summary>
    private readonly ValuerEngine _engine =
        engine ?? throw new ArgumentNullException(nameof(engine));

    private ValuerChainer NextChainer(ValuerMod? optionConfiguration)
    {
        return (optionConfiguration == null) ? this : this;
    }

    /// <summary>If <paramref name="item"/> can be tracked in history.</summary>
    /// <param name="item">Item to check.</param>
    /// <returns><c>true</c> if tracking <paramref name="item"/> is possible; <c>false</c> otherwise.</returns>
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
                return _engine.Compare(expected, actual, NextChainer(optionConfiguration));
            }
            else
            {
                return [];
            }
        }
        else
        {
            return _engine.Compare(expected, actual, NextChainer(optionConfiguration));
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Difference>> CompareAsync(
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
                    return await _engine
                        .CompareAsync(expected, actual, NextChainer(optionConfiguration))
                        .ConfigureAwait(false);
                }
                finally
                {
                    _ = _compareHistory.Remove(refHash);
                }
            }
            else
            {
                return [];
            }
        }
        else
        {
            return await _engine
                .CompareAsync(expected, actual, NextChainer(optionConfiguration))
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item)
    {
        return GetHashCode(item, null);
    }

    /// <inheritdoc/>
    public int GetHashCode(object? item, ValuerMod? optionConfiguration = null)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        if (!CanTrack(item))
        {
            return _engine.GetHashCode(item, this);
        }

        int refHash = RuntimeHelpers.GetHashCode(item);
        if (_hashHistory.TryGetValue(refHash, out object? stored) && ReferenceEquals(item, stored))
        {
            return 0;
        }

        _hashHistory[refHash] = item;
        try
        {
            return _engine.GetHashCode(item, this);
        }
        finally
        {
            _ = _hashHistory.Remove(refHash);
        }
    }

    /// <inheritdoc/>
    public async Task<int> GetHashCodeAsync(object? item, ValuerMod? optionConfiguration = null)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        if (!CanTrack(item))
        {
            return await _engine.GetHashCodeAsync(item, this).ConfigureAwait(false);
        }

        int refHash = RuntimeHelpers.GetHashCode(item);
        if (_hashHistory.TryGetValue(refHash, out object? stored) && ReferenceEquals(item, stored))
        {
            return 0;
        }

        _hashHistory[refHash] = item;
        try
        {
            return await _engine.GetHashCodeAsync(item, this).ConfigureAwait(false);
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
    public bool Equals(object? x, object? y, ValuerMod? optionConfiguration = null)
    {
        return !Compare(x, y, optionConfiguration).Any();
    }

    /// <inheritdoc/>
    public async Task<bool> EqualsAsync(object? x, object? y, ValuerMod? optionConfiguration = null)
    {
        return !(await CompareAsync(x, y, optionConfiguration).ConfigureAwait(false)).Any();
    }
}
