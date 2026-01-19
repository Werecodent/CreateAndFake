using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tooling;

/// <summary>Runs the hint behavior pipeline.</summary>
/// <typeparam name="THint">Hint type being used.</typeparam>
/// <param name="defaultHints">Hints setup by the framework.</param>
public abstract class ToolEngine<THint>(IEnumerable<THint> defaultHints) : IToolEngine<THint>
    where THint : IToolHint
{
    /// <inheritdoc/>
    public virtual IEnumerable<Type> SupportedTypes =>
        defaultHints.SelectMany(h => h.SupportedTypes);

    /// <summary>Picks hints to use for the tool based upon chainer options.</summary>
    /// <typeparam name="TOptions">Test</typeparam>
    /// <param name="chainer">Potentially modified configuration to use.</param>
    /// <returns>Hints to utilize.</returns>
    protected IEnumerable<THint> SelectHints<TOptions>(IToolChainer<TOptions> chainer)
        where TOptions : IToolHintOptions<TOptions, THint>
    {
        ArgumentGuard.ThrowIfNull(chainer);

        foreach (THint hint in chainer.Options.Hints)
        {
            yield return hint;
        }
        if (chainer.Options.IncludeDefaultHints)
        {
            foreach (THint hint in defaultHints ?? [])
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
