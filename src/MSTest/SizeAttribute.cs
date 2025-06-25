using CreateAndFake.RunnerTool.Attributes;

namespace CreateAndFake.MSTest;

/// <inheritdoc/>
public sealed class SizeAttribute(int count) : BaseSizeAttribute(count) { }
