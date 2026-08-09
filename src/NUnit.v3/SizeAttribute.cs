using Werecodent.CreateAndFake.RunnerTool.Attributes;

namespace Werecodent.CreateAndFake.NUnit.v3;

/// <inheritdoc/>
public sealed class SizeAttribute(int count) : BaseSizeAttribute(count);
