using CreateAndFake.RunnerTool.Attributes;

namespace CreateAndFake.xUnit.v2;

/// <inheritdoc/>
public sealed class SizeAttribute(int count) : BaseSizeAttribute(count);
