using CreateAndFake.RunnerTool.Attributes;

namespace CreateAndFake.NUnit;

/// <inheritdoc/>
public sealed class SizeAttribute(int count) : BaseSizeAttribute(count) { }
