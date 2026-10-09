using Werecodent.CreateAndFake.ExtractorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class ObjectExtractHintTests : ExtractHintTestBase<ObjectExtractHint>
{
    private static readonly Type[] _ValidTypes = [typeof(DataHolderSample)];

    public ObjectExtractHintTests()
        : base(_ValidTypes, []) { }
}
