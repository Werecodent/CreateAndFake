using Werecodent.CreateAndFake.Design.Tooling;

namespace Werecodent.CreateAndFake.ValuerTool.Engine;

/// <inheritdoc cref="IValuer"/>
public interface IValuerChainer : IValuer, IToolChainer<ValuerOptions, ICompareHint>;
