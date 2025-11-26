using CreateAndFake.RunnerTool.Attributes;

namespace CreateAndFake.MSTest.v4;

/// <inheritdoc/>
public sealed class SizeAttribute(int count) : BaseSizeAttribute(count) { }
