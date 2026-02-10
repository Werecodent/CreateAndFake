using CreateAndFake.Design.Tooling;

namespace CreateAndFake.MutatorTool.Engine;

/// <inheritdoc/>
public interface IMutatorEngine : IToolEngine<IMutateHint>
{
    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IMutator.Variant"/>
    object Variant(Type type, object? instance, IMutatorChainer chainer);

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IMutator.VariantOf"/>
    object VariantOf(Type type, IEnumerable<object?> instances, IMutatorChainer chainer);

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IMutator.Unique"/>
    object Unique(Type type, object? instance, IMutatorChainer chainer);

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IMutator.UniqueOf"/>
    object UniqueOf(Type type, IEnumerable<object?> instances, IMutatorChainer chainer);

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IMutator.Modify"/>
    bool Modify(object? instance, IMutatorChainer chainer);
}
