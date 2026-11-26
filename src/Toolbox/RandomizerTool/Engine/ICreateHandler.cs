using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.RandomizerTool.Engine;

/// <summary>Handles creation of a concrete <see cref="Type"/>.</summary>
/// <remarks>For optimized performance by keying off the <see cref="ITypeSupporter.SupportedType"/>.</remarks>
internal interface ICreateHandler : ITypeSupporter
{
    /// <summary>Creates a random instance of the <see cref="ITypeSupporter.SupportedType"/>.</summary>
    /// <param name="randomizer">Handles randomizing supporting values.</param>
    /// <returns>The created instance.</returns>
    object? CreateSupported(IRandomizerChainer randomizer);
}
