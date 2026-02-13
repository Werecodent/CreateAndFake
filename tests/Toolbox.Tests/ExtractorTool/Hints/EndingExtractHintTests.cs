using CreateAndFake.ExtractorTool.Engine;
using CreateAndFake.ExtractorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class EndingExtractHintTests : ExtractHintTestBase<EndingExtractHint>
{
    private static readonly EndingExtractHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(ExtractPriority)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public EndingExtractHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
