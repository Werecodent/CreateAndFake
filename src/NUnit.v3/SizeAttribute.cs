using CreateAndFake.RunnerTool.Attributes;

namespace CreateAndFake.NUnit.v3;

/// <inheritdoc/>
public sealed class SizeAttribute(int count) : BaseSizeAttribute(count);
