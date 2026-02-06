using CreateAndFake.Design.Tooling;

namespace CreateAndFake.MutatorTool.Engine;

/// <summary>Handles mutation of specific types.</summary>
public interface IMutateHint : IToolHint
{
    /// <inheritdoc cref="IMutatorEngine.Modify(object?, IMutatorChainer)"/>
    MutateHintResult TryModifying(object instance, IMutatorChainer chainer);
}
