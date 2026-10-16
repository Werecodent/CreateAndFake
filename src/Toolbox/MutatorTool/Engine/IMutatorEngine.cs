using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.MutatorTool.Engine;

/// <inheritdoc/>
public interface IMutatorEngine : IToolEngine<IMutateHint>
{
    /// <inheritdoc cref="IMutator.Variant"/>
    /// <inheritdoc cref="Modify"/>
    object Variant(Type type, object? instance, IMutatorChainer chainer);

    /// <inheritdoc cref="Variant"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<object> VariantAsync(
        Type type,
        object? instance,
        IMutatorChainer chainer,
        CancellationToken canceler
    );

    /// <inheritdoc cref="IMutator.VariantOf"/>
    /// <inheritdoc cref="Modify"/>
    object VariantOf(Type type, IEnumerable<object?> instances, IMutatorChainer chainer);

    /// <inheritdoc cref="VariantOf"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<object> VariantOfAsync(
        Type type,
        IEnumerable<object?> instances,
        IMutatorChainer chainer,
        CancellationToken canceler
    );

    /// <inheritdoc cref="IMutator.Unique"/>
    /// <inheritdoc cref="Modify"/>
    object Unique(Type type, object? instance, IMutatorChainer chainer);

    /// <inheritdoc cref="Unique"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<object> UniqueAsync(
        Type type,
        object? instance,
        IMutatorChainer chainer,
        CancellationToken canceler
    );

    /// <inheritdoc cref="IMutator.UniqueOf"/>
    /// <inheritdoc cref="Modify"/>
    object UniqueOf(Type type, IEnumerable<object?> instances, IMutatorChainer chainer);

    /// <inheritdoc cref="UniqueOf"/>
    /// <param name="canceler">Aborts execution if triggered.</param>
    Task<object> UniqueOfAsync(
        Type type,
        IEnumerable<object?> instances,
        IMutatorChainer chainer,
        CancellationToken canceler
    );

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IMutator.Modify"/>
    bool Modify(object? instance, IMutatorChainer chainer);
}
