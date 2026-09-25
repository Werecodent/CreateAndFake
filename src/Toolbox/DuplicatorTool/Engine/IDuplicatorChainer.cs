using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.DuplicatorTool.Engine;

/// <inheritdoc cref="IDuplicator"/>
public interface IDuplicatorChainer : IDuplicator, IToolChainer<DuplicatorOptions, ICopyHint>
{
    /// <summary>Adds successful clone details to history.</summary>
    /// <param name="source">Object cloned.</param>
    /// <param name="clone">The clone.</param>
    void AddToHistory(object source, object clone);
}
