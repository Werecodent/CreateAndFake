using CreateAndFake.Design.Content;

namespace CreateAndFake.DuplicatorTool.Engine;

/// <summary>Handles cloning of a specific type.</summary>
internal interface ICopyHandler : ITypeSupporter
{
    /// <summary>Copies an instance of the specific type.</summary>
    /// <param name="source">Object to clone.</param>
    /// <param name="duplicator">Handles cloning child values.</param>
    /// <returns>The clone.</returns>
    object? CopySupported(object source, IDuplicatorChainer duplicator);
}
