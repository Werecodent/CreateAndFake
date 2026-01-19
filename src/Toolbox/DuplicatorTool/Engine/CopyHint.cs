using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.DuplicatorTool.Engine;

/// <summary>Handles cloning specific types for <see cref="IDuplicator"/> .</summary>
public abstract class CopyHint : IToolHint
{
    /// <inheritdoc/>
    public virtual IEnumerable<Type> SupportedTypes { get; } = [];

    /// <summary>Tries to deep clone <paramref name="source"/>.</summary>
    /// <param name="source">Object to clone.</param>
    /// <param name="duplicator">Handles cloning child values.</param>
    /// <returns>Possible result.</returns>
    public abstract CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator);

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}
