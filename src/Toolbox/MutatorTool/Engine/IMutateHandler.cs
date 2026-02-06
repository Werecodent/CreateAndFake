using CreateAndFake.Design.Content;

namespace CreateAndFake.MutatorTool.Engine;

/// <inheritdoc/>
public interface IMutateHandler : ITypeSupporter
{
    /// <inheritdoc cref="IMutateHint.TryModifying(object, IMutatorChainer)"/>
    bool ModifySupported(object instance, IMutatorChainer mutator);
}
