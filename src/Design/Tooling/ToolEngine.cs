using System.Collections.Frozen;
using System.Collections.Immutable;
using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tooling;

/// <inheritdoc cref="IToolEngine{T}"/>
public abstract class ToolEngine<THint> : IToolEngine<THint>
    where THint : IToolHint
{
    /// <summary>All found hints from all loaded assemblies.</summary>
    private static readonly ImmutableArray<THint> _AllHints =
    [
        .. TypeDescriber
            .FindLoadedSubclasses<THint>()
            .Select(Activator.CreateInstance)
            .Cast<THint>()
            .Where(h => h.EnginePriority != int.MinValue)
            .OrderByDescending(h => h.EnginePriority),
    ];

    /// <summary>All <c>CreateAndFake</c> hints.</summary>
    private static readonly FrozenSet<THint> _FrameworkHints = _AllHints
        .Where(h => h.GetType().Assembly == typeof(THint).Assembly)
        .ToFrozenSet();

    /// <inheritdoc/>
    public virtual IEnumerable<Type> SupportedTypes => _AllHints.SelectMany(h => h.SupportedTypes);

    /// <summary>Picks hints to use for the tool based upon configured options.</summary>
    /// <typeparam name="TOptions">
    ///     The <see cref="Type"/> managing configuration options.
    /// </typeparam>
    /// <param name="chainer">Holds the configured options to use.</param>
    /// <returns>The selected hints to utilize in the order returned.</returns>
    protected static IEnumerable<THint> SelectHints<TOptions>(IToolChainer<TOptions, THint> chainer)
        where TOptions : IToolHintOptions<TOptions, THint>
    {
        ArgumentGuard.ThrowIfNull(chainer);

        foreach (THint hint in chainer.Options.Hints)
        {
            yield return hint;
        }
        foreach (THint hint in _AllHints)
        {
            if (_FrameworkHints.Contains(hint))
            {
                if (chainer.Options.IncludeFrameworkHints)
                {
                    yield return hint;
                }
            }
            else if (chainer.Options.IncludeFoundHints)
            {
                yield return hint;
            }
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
