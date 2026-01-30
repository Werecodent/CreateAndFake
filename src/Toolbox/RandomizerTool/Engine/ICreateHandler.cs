using CreateAndFake.Design.Content;

namespace CreateAndFake.RandomizerTool.Engine;

/// <summary>Handles creation of a specific type.</summary>
internal interface ICreateHandler : ITypeSupporter
{
    /// <summary>
    ///     Creates a random instance of the <see cref="ITypeSupporter.SupportedType"/>.
    /// </summary>
    /// <param name="randomizer">Handles randomizing supporting values.</param>
    /// <returns>The created instance.</returns>
    object? CreateSupported(IRandomizerChainer randomizer);
}
