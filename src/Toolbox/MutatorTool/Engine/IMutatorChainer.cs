using CreateAndFake.Design.Tooling;

namespace CreateAndFake.MutatorTool.Engine;

/// <summary>Provides a callback into <see cref="IMutator"/> to mutate child values.</summary>
public interface IMutatorChainer : IMutator, IToolChainer<MutatorOptions>;
