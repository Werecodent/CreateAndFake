using CreateAndFake.RunnerTool.Attributes;

namespace CreateAndFake.MSTest.v3;

/// <inheritdoc/>
public sealed class SizeAttribute(int count) : BaseSizeAttribute(count) { }
