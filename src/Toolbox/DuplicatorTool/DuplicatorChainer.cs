using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CreateAndFake.FakerTool.Proxy;

#pragma warning disable IDE0028 // Collection initialization can be simplified: Invalid because it's not constructible.

namespace CreateAndFake.DuplicatorTool;

/// <summary>Provides a callback into <see cref="IDuplicator"/> to create child values.</summary>
/// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
/// <param name="duplicator"><inheritdoc cref="Duplicator" path="/summary"/></param>
/// <param name="callback"><inheritdoc cref="_callback" path="/summary"/></param>
public sealed class DuplicatorChainer(
    DuplicatorOptions options,
    IDuplicator duplicator,
    Func<object?, DuplicatorChainer, object?> callback
)
{
    /// <summary>Reference to the actual duplicator.</summary>
    internal IDuplicator Duplicator { get; } =
        duplicator ?? throw new ArgumentNullException(nameof(duplicator));

    /// <summary>Callback to the duplicator to handle child values.</summary>
    private readonly Func<object?, DuplicatorChainer, object?> _callback =
        callback ?? throw new ArgumentNullException(nameof(callback));

    /// <summary>History of clones to match up references.</summary>
    private readonly ConditionalWeakTable<object, object?> _history = new();

    /// <inheritdoc cref="DuplicatorOptions"/>
    public DuplicatorOptions Options { get; } = options;

    /// <summary>Adds successful clone details to history.</summary>
    /// <param name="source">Object cloned.</param>
    /// <param name="clone">The clone.</param>
    public void AddToHistory(object source, object clone)
    {
        if (CanTrack(source))
        {
            _history.Add(source, clone);
        }
    }

    /// <typeparam name="T"><c>Type</c> being cloned.</typeparam>
    /// <inheritdoc cref="Copy"/>
    public T? Copy<T>(T? source)
    {
        return (T?)Copy((object?)source);
    }

    /// <summary>Deep clones <paramref name="source"/>.</summary>
    /// <param name="source">Object to clone.</param>
    /// <returns>Clone of <paramref name="source"/>.</returns>
    /// <exception cref="NotSupportedException">If no hint supports cloning <paramref name="source"/>.</exception>
    public object? Copy(object? source)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        if (!CanTrack(source))
        {
            return _callback.Invoke(source, this);
        }

        if (_history.TryGetValue(source, out object? clone))
        {
            return clone;
        }

        object? result = _callback.Invoke(source, this);
        if (!_history.TryGetValue(source, out _))
        {
            _history.Add(source, result);
        }
        return result;
    }

    /// <summary>If <paramref name="source"/> can be tracked in history.</summary>
    /// <param name="source">Item to check.</param>
    /// <returns><c>true</c> if possible; <c>false</c> otherwise.</returns>
    private static bool CanTrack([NotNullWhen(true)] object? source)
    {
        return !(source == null || source is IFaked || source.GetType().IsValueType);
    }
}

#pragma warning restore IDE0028 // Collection initialization can be simplified
