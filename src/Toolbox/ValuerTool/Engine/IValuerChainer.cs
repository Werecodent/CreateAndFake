using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ValuerTool.Engine;

/// <inheritdoc cref="IValuer"/>
public interface IValuerChainer : IValuer, IToolChainer<ValuerOptions>;
