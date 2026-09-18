using Werecodent.CreateAndFake.ExtractorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class ObjectExtractHintTests : ExtractHintTestBase<ObjectExtractHint>
{
    private static readonly ObjectExtractHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(DataHolderSample)];

    private static readonly Type[] _InvalidTypes = [];

    public ObjectExtractHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
