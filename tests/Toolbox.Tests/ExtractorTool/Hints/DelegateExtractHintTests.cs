using Werecodent.CreateAndFake.ExtractorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class DelegateExtractHintTests : ExtractHintTestBase<DelegateExtractHint>
{
    private static readonly DelegateExtractHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(Delegate)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public DelegateExtractHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
