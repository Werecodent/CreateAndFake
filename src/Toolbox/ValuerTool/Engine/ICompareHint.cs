using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.ValuerTool.Engine;

/// <summary>Handles comparisons of the <see cref="IToolHint.SupportedTypes"/>.</summary>
public interface ICompareHint : IToolHint
{
    /// <summary>
    ///     Tries to find the differences between <paramref name="expected"/> and <paramref name="actual"/>.
    /// </summary>
    /// <param name="expected">Object to compare with <paramref name="actual"/>.</param>
    /// <param name="actual">Potentially different object to compare against <paramref name="expected"/>.</param>
    /// <param name="chainer">Handles comparing child values.</param>
    /// <returns>If the hint supported the operation with the attempt result if so.</returns>
    DifferenceHintResult TryToCompare(object expected, object actual, IValuerChainer chainer);

    /// <inheritdoc cref="TryToCompare"/>
    DifferenceHintAsyncResult TryToAsyncCompare(
        object expected,
        object actual,
        IValuerChainer chainer,
        CancellationToken canceler
    );

    /// <summary>Tries to compute an identifying hash code for <paramref name="item"/> based upon value.</summary>
    /// <param name="item">Object to generate a hash code for.</param>
    /// <param name="chainer">Handles hashing behavior for child values.</param>
    /// <returns>If the hint supported the operation with the attempt result if so.</returns>
    HashCodeHintResult TryToGetHashCode(object item, IValuerChainer chainer);

    /// <inheritdoc cref="TryToGetHashCode"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    HashCodeHintAsyncResult TryToAsyncGetHashCode(
        object item,
        IValuerChainer chainer,
        CancellationToken canceler
    );
}
