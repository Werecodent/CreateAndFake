using CreateAndFake.ExtractorTool.Hints;

namespace CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class NullExtractHintTests : ExtractHintTestBase<NullExtractHint>
{
    private static readonly NullExtractHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [];

    private static readonly Type[] _InvalidTypes = [];

    public NullExtractHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
