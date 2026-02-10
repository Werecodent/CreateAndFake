using CreateAndFake.Design.Tooling;

namespace CreateAndFake.MutatorTool.Engine;

/// <inheritdoc cref="IToolChainer{T,T}"/>
/// <remarks>Provides a callback into <see cref="IMutator"/> to mutate child values.</remarks>
public interface IMutatorChainer : IMutator, IToolChainer<MutatorOptions, IMutateHint>;
