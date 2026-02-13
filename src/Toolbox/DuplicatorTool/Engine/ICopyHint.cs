using CreateAndFake.Design.Tooling;

namespace CreateAndFake.DuplicatorTool.Engine;

/// <summary>Handles cloning of the <see cref="IToolHint.SupportedTypes"/>.</summary>
public interface ICopyHint : IToolHint
{
    /// <summary>Tries to deep clone <paramref name="source"/>.</summary>
    /// <param name="source">Object to clone.</param>
    /// <param name="duplicator">Handles cloning child values.</param>
    /// <returns>Possible result.</returns>
    CopyHintResult TryCopy(object source, IDuplicatorChainer duplicator);
}
