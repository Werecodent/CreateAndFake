using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CreateAndFake.Toolbox.FakerTool.Proxy;

namespace CreateAndFake.Toolbox.ValuerTool;

/// <summary>Provides a callback into <see cref="IValuer"/> to create child values.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <param name="valuer"><inheritdoc cref="Valuer" path="/summary"/></param>
/// <param name="hasher"><inheritdoc cref="_hasher" path="/summary"/></param>
/// <param name="comparer"><inheritdoc cref="_comparer" path="/summary"/></param>
public sealed class ValuerChainer(
    ValuerOptions options,
    IValuer valuer,
    Func<object?, ValuerChainer, int> hasher,
    Func<object?, object?, ValuerChainer, IEnumerable<Difference>> comparer)
{
    /// <summary>Callback to <see cref="IValuer"/> to handle child hashes.</summary>
    private readonly Func<object?, ValuerChainer, int> _hasher
        = hasher ?? throw new ArgumentNullException(nameof(hasher));

    /// <summary>Callback to <see cref="IValuer"/> to handle child comparisons.</summary>
    private readonly Func<object?, object?, ValuerChainer, IEnumerable<Difference>> _comparer
        = comparer ?? throw new ArgumentNullException(nameof(comparer));

    /// <summary>History of hashes to match up references.</summary>
    private readonly Dictionary<int, object?> _hashHistory = [];

    /// <summary>History of comparisons to match up references.</summary>
    private readonly HashSet<(int, int)> _compareHistory = [];

    /// <summary>Reference to the actual source valuer.</summary>
    internal IValuer Valuer { get; } = valuer ?? throw new ArgumentNullException(nameof(valuer));

    /// <inheritdoc cref="ValuerOptions"/>
    public ValuerOptions Options { get; } = options;

    /// <inheritdoc cref="IValuer.Compare"/>
    public IEnumerable<Difference> Compare(object? expected, object? actual)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        if (CanTrack(expected) && CanTrack(actual))
        {
            (int, int) refHash = (RuntimeHelpers.GetHashCode(expected), RuntimeHelpers.GetHashCode(actual));

            return _compareHistory.Add(refHash)
                ? _comparer.Invoke(expected, actual, this)
                : [];
        }
        else
        {
            return _comparer.Invoke(expected, actual, this);
        }
    }

    /// <inheritdoc cref="IValuer.Equals(object,object)"/>
    public new bool Equals(object? x, object? y)
    {
        return !Compare(x, y).Any();
    }

    /// <inheritdoc cref="IValuer.GetHashCode(object)"/>
    public int GetHashCode(object? item)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        if (!CanTrack(item))
        {
            return _hasher.Invoke(item, this);
        }

        int refHash = RuntimeHelpers.GetHashCode(item);
        if (_hashHistory.TryGetValue(refHash, out object? stored) && ReferenceEquals(item, stored))
        {
            return 0;
        }

        _hashHistory[refHash] = item;
        try
        {
            return _hasher.Invoke(item, this);
        }
        finally
        {
            _ = _hashHistory.Remove(refHash);
        }
    }

    /// <summary>If <paramref name="item"/> can be tracked in history.</summary>
    /// <param name="item">Item to check.</param>
    /// <returns><c>true</c> if tracking <paramref name="item"/> is possible; <c>false</c> otherwise.</returns>
    private static bool CanTrack([NotNullWhen(true)] object? item)
    {
        return !(item == null || item is IFaked || item.GetType().IsValueType);
    }
}
