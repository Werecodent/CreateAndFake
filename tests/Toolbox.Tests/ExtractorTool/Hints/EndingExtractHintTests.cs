using Werecodent.CreateAndFake.ExtractorTool.Engine;
using Werecodent.CreateAndFake.ExtractorTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class EndingExtractHintTests : ExtractHintTestBase<EndingExtractHint>
{
    private static readonly Type[] _ValidTypes = [typeof(ExtractPriority)];

    public EndingExtractHintTests()
        : base(_ValidTypes) { }
}
