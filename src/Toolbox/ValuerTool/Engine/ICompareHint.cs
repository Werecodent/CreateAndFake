using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ValuerTool.Engine;

/// <summary>Handles comparing specific types for <see cref="IValuer"/>.</summary>
public interface ICompareHint : IToolHint
{
    /// <summary>
    ///     Tries to find the differences between <paramref name="expected"/> and <paramref name="actual"/>.
    /// </summary>
    /// <param name="expected">Object to compare with <paramref name="actual"/>.</param>
    /// <param name="actual">Potentially different object to compare against <paramref name="expected"/>.</param>
    /// <param name="valuer">Handles comparing child values.</param>
    /// <returns>Possible result.</returns>
    DifferenceHintResult TryCompare(object expected, object actual, IValuerChainer valuer);

    /// <inheritdoc cref="TryCompare"/>
    DifferenceHintAsyncResult TryAsyncCompare(
        object expected,
        object actual,
        IValuerChainer valuer
    );

    /// <summary>Tries to compute an identifying hash code for <paramref name="item"/> based upon value.</summary>
    /// <param name="item">Object to generate a hash code for.</param>
    /// <param name="valuer">Handles hashing behavior for child values.</param>
    /// <returns>Possible result.</returns>
    HashCodeHintResult TryGetHashCode(object item, IValuerChainer valuer);

    /// <inheritdoc cref="TryGetHashCode"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    HashCodeHintAsyncResult TryAsyncGetHashCode(
        object item,
        IValuerChainer valuer,
        CancellationToken canceler
    );
}
