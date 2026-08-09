using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.MSTest.v4;

/// <inheritdoc/>
public sealed class SizeAttribute(int count) : BaseSizeAttribute(count);
