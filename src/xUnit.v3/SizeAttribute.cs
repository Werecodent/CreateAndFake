using CreateAndFake.RunnerTool.Attributes;

namespace CreateAndFake.xUnit.v3;

/// <inheritdoc/>
public sealed class SizeAttribute(int count) : BaseSizeAttribute(count) { }
