using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Randomization;

/// <summary>Handle randomizing a specific value type.</summary>
internal interface IValueHandler : ITypeSupporter
{
    /// <summary>
    ///     Generates a random value of the <see cref="ITypeSupporter.SupportedType"/>.
    /// </summary>
    /// <param name="gen">Handles randomizing supporting values.</param>
    /// <returns>The generated value.</returns>
    object CreateSupported(IRandom gen);

    /// <summary>
    ///     Generates a constrained value of the <see cref="ITypeSupporter.SupportedType"/>.
    /// </summary>
    /// <param name="percent">Random [0,1) value for calculating the value.</param>
    /// <inheritdoc cref="IRandom.Next{T}(T, T)"/>
    object CreateSupported(object min, object max, double percent);
}
