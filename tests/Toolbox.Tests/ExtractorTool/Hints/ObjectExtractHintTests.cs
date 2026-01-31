using CreateAndFake.ExtractorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class ObjectExtractHintTests : ExtractHintTestBase<ObjectExtractHint>
{
    private static readonly ObjectExtractHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(DataHolderSample)];

    private static readonly Type[] _InvalidTypes = [];

    public ObjectExtractHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
