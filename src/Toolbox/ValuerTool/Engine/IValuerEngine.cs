using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ValuerTool.Engine;

/// <inheritdoc cref="IValuer"/>
public interface IValuerEngine : IToolEngine<ICompareHint>
{
    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IValuer.Compare(object,object,ValuerMod)"/>
    IEnumerable<Difference> Compare(object? expected, object? actual, IValuerChainer chainer);

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IValuer.GetHashCodeAsync"/>
    IAsyncEnumerable<Difference> CompareAsync(
        object? expected,
        object? actual,
        IValuerChainer chainer,
        CancellationToken canceler
    );

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IValuer.GetHashCode(object)"/>
    int GetHashCode(object? item, IValuerChainer chainer);

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IValuer.GetHashCodeAsync"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<int> GetHashCodeAsync(object? item, IValuerChainer chainer, CancellationToken canceler);
}
