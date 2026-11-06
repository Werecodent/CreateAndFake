using Werecodent.CreateAndFake.Design.Types;

namespace Werecodent.CreateAndFake.ValuerTool.Engine;

/// <summary>Handles comparison and hashing of a specific type.</summary>
internal interface ICompareHandler : ITypeSupporter
{
    /// <summary>
    ///     Compares instances of the <see cref="ITypeSupporter.SupportedType"/>.
    /// </summary>
    /// <inheritdoc cref="CompareHint.Compare(object?, object?, IValuerChainer)"/>
    IEnumerable<Difference> CompareSupported(
        object expected,
        object actual,
        IValuerChainer chainer
    );

    /// <summary>
    ///     Computes the identifying hash code for the <see cref="ITypeSupporter.SupportedType"/>.
    /// </summary>
    /// <inheritdoc cref="CompareHint.GetHashCode(object?, IValuerChainer)"/>
    int HashSupported(object item, IValuerChainer chainer);
}
