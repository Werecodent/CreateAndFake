using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.MutatorTool.Engine;

/// <summary>Handles mutation of the <see cref="IToolHint.SupportedTypes"/>.</summary>
public interface IMutateHint : IToolHint
{
    /// <returns>If the hint supported the operation with the attempt result if so.</returns>
    /// <inheritdoc cref="IMutatorEngine.Modify(object?, IMutatorChainer)"/>
    MutateHintResult TryToModify(object instance, IMutatorChainer chainer);
}
