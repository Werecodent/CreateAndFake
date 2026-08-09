using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.xUnit.v2;

/// <inheritdoc/>
public sealed class SizeAttribute(int count) : BaseSizeAttribute(count);
