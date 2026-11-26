using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.DuplicatorTool.Engine;

/// <summary>Handles cloning of a concrete <see cref="Type"/>.</summary>
/// <remarks>For optimized performance by keying off the <see cref="ITypeSupporter.SupportedType"/>.</remarks>
internal interface ICopyHandler : ITypeSupporter
{
    /// <summary>Copies an instance of the specific type.</summary>
    /// <param name="source">Object to clone.</param>
    /// <param name="duplicator">Handles cloning child values.</param>
    /// <returns>The clone.</returns>
    object? CopySupported(object source, IDuplicatorChainer duplicator);
}
