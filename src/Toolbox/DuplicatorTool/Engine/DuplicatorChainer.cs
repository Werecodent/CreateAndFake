using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CreateAndFake.Design;
using CreateAndFake.FakerTool.Proxy;

namespace CreateAndFake.DuplicatorTool.Engine;

#pragma warning disable IDE0028 // Invalid because it's not constructible.

/// <summary>Provides a callback into <see cref="IDuplicator"/> to create child values.</summary>
public sealed class DuplicatorChainer : IDuplicatorChainer
{
    /// <summary>Callback mechanism.</summary>
    private readonly IDuplicatorEngine _engine;

    /// <summary>History of clones to match up references.</summary>
    private readonly ConditionalWeakTable<object, object?> _history;

    /// <inheritdoc cref="DuplicatorOptions"/>
    public DuplicatorOptions Options { get; }

    /// <inheritdoc cref="IDuplicatorChainer"/>
    /// <param name="options"><inheritdoc cref="Options" path="/summary"/></param>
    /// <param name="engine"><inheritdoc cref="_engine" path="/summary"/></param>
    public DuplicatorChainer(DuplicatorOptions options, IDuplicatorEngine engine)
    {
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        Options = options ?? throw new ArgumentNullException(nameof(options));

        _history = new ConditionalWeakTable<object, object?>();
    }

    /// <inheritdoc cref="IDuplicatorChainer"/>
    /// <param name="prevChainer">Previous chainer to build upon.</param>
    /// <param name="optionConfiguration">Modifications of <see cref="Options"/> for the new tool.</param>
    private DuplicatorChainer(DuplicatorChainer prevChainer, DuplicatorMod? optionConfiguration)
    {
        DuplicatorOptions options = prevChainer.Options;

        Options = (optionConfiguration != null) ? optionConfiguration.Invoke(options) : options;

        _engine = prevChainer._engine;
        _history = prevChainer._history;
    }

    private DuplicatorChainer GetSubChainer(DuplicatorMod? optionConfiguration)
    {
        return (optionConfiguration != null)
            ? new DuplicatorChainer(this, optionConfiguration)
            : this;
    }

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

    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(source))]
    public T Copy<T>(T source, DuplicatorMod? optionConfiguration = null)
    {
        return (T?)Copy((object?)source, optionConfiguration)!;
    }

    /// <inheritdoc cref="Copy{T}"/>
    [return: NotNullIfNotNull(nameof(source))]
    public object? Copy(object? source, DuplicatorMod? optionConfiguration = null)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        if (!CanTrack(source))
        {
            return _engine.Copy(source, GetSubChainer(optionConfiguration));
        }

        if (_history.TryGetValue(source, out object? clone))
        {
            return clone!;
        }

        object? result = _engine.Copy(source, GetSubChainer(optionConfiguration));
        if (!_history.TryGetValue(source, out _))
        {
            _history.Add(source, result);
        }
        return result;
    }

    /// <summary>If <paramref name="source"/> can be tracked in history.</summary>
    /// <param name="source">Item to check.</param>
    /// <returns><see langword="true"/> if possible, <see langword="false"/> otherwise.</returns>
    private static bool CanTrack([NotNullWhen(true)] object? source)
    {
        return !(source == null || source is IFaked || source.GetType().IsValueType);
    }

    /// <inheritdoc/>
    public IDuplicator WithOptions(DuplicatorMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration, nameof(optionConfiguration));
        return new DuplicatorChainer(this, optionConfiguration);
    }
}

#pragma warning restore IDE0028
