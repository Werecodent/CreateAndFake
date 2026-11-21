using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.DuplicatorTool.Engine;

#pragma warning disable IDE0028 // Invalid because it's not constructible.

/// <summary>Provides a callback into <see cref="IDuplicator"/> to create child values.</summary>
public sealed class DuplicatorChainer
    : ToolChainer<DuplicatorChainer, IDuplicatorEngine, DuplicatorOptions, ICopyHint>,
        IDuplicatorChainer
{
    /// <summary>History of clones to match up references.</summary>
    private readonly ConditionalWeakTable<object, object?> _history;

    /// <inheritdoc/>
    public DuplicatorChainer(DuplicatorOptions options, IDuplicatorEngine engine)
        : base(options, engine)
    {
        _history = new ConditionalWeakTable<object, object?>();
    }

    /// <inheritdoc/>
    private DuplicatorChainer(DuplicatorOptions options, DuplicatorChainer prevChainer)
        : base(options, prevChainer)
    {
        _history = prevChainer._history;
    }

    /// <inheritdoc/>
    protected override DuplicatorChainer CreateSubChainer(DuplicatorOptions subOptions)
    {
        return new DuplicatorChainer(subOptions, this);
    }

    /// <inheritdoc/>
    public object? AddToHistory(object source, object clone)
    {
        if (CanTrack(source))
        {
            if (_history.TryGetValue(source, out object? existingClone))
            {
                return existingClone;
            }
            _history.Add(source, clone);
        }
        return null;
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
        if (!CanTrack(source))
        {
            return Engine.Copy(source, GetSubChainer(optionConfiguration));
        }

        if (_history.TryGetValue(source, out object? clone))
        {
            return clone!;
        }

        object? result = Engine.Copy(source, GetSubChainer(optionConfiguration));
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
        return source?.GetType().IsValueType == false;
    }

    /// <inheritdoc/>
    public IDuplicator WithOptions(DuplicatorMod optionConfiguration)
    {
        ArgumentGuard.ThrowIfNull(optionConfiguration);
        return new DuplicatorChainer(optionConfiguration.Invoke(Options), this);
    }
}

#pragma warning restore
