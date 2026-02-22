using CreateAndFake.Design.Types;

namespace CreateAndFake.Design.Randomization.Handlers;

/// <summary>Handle randomizing a specific value type.</summary>
internal interface IValueHandler : ITypeSupporter
{
    /// <summary>
    ///     Generates a random value of the <see cref="ITypeSupporter.SupportedType"/>.
    /// </summary>
    /// <param name="gen">Handles randomizing supporting values.</param>
    /// <inheritdoc cref="IRandom.Next{T}()"/>
    object CreateSupported(IRandom gen);

    /// <summary>
    ///     Generates a positive constrained value of
    ///     the <see cref="ITypeSupporter.SupportedType"/>.
    /// </summary>
    /// <param name="gen">Handles randomizing supporting values.</param>
    /// <inheritdoc cref="IRandom.Next{T}(T)"/>
    object CreateSupported(IRandom gen, object max);

    /// <summary>
    ///     Generates a constrained value of the <see cref="ITypeSupporter.SupportedType"/>.
    /// </summary>
    /// <param name="gen">Handles randomizing supporting values.</param>
    /// <inheritdoc cref="IRandom.Next{T}(T, T)"/>
    object CreateSupported(IRandom gen, object min, object max);
}
