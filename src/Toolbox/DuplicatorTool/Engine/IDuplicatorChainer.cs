using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.DuplicatorTool.Engine;

/// <inheritdoc cref="IDuplicator"/>
public interface IDuplicatorChainer : IDuplicator, IToolChainer<DuplicatorOptions, ICopyHint>
{
    /// <summary>Adds successful clone details to history.</summary>
    /// <param name="source">Object cloned.</param>
    /// <param name="clone">The clone.</param>
    /// <returns>Existing clone if there already was one created for <paramref name="source"/>.</returns>
    object? AddToHistory(object source, object clone);
}
