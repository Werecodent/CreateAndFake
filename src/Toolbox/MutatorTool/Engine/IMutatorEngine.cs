using CreateAndFake.Design.Tooling;

namespace CreateAndFake.MutatorTool.Engine;

/// <inheritdoc/>
public interface IMutatorEngine : IToolEngine<IMutateHint>
{
    /// <inheritdoc cref="IMutator.Variant"/>
    /// <inheritdoc cref="Modify"/>
    object Variant(Type type, object? instance, IMutatorChainer chainer);

    /// <inheritdoc cref="IMutator.VariantOf"/>
    /// <inheritdoc cref="Modify"/>
    object VariantOf(Type type, IEnumerable<object?> instances, IMutatorChainer chainer);

    /// <inheritdoc cref="IMutator.Unique"/>
    /// <inheritdoc cref="Modify"/>
    object Unique(Type type, object? instance, IMutatorChainer chainer);

    /// <inheritdoc cref="IMutator.UniqueOf"/>
    /// <inheritdoc cref="Modify"/>
    object UniqueOf(Type type, IEnumerable<object?> instances, IMutatorChainer chainer);

    /// <param name="chainer">Handles callback behavior for child values.</param>
    /// <inheritdoc cref="IMutator.Modify"/>
    bool Modify(object? instance, IMutatorChainer chainer);
}
