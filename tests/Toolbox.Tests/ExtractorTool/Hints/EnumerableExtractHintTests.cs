using Werecodent.CreateAndFake.ExtractorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class EnumerableExtractHintTests : ExtractHintTestBase<EnumerableExtractHint>
{
    private static readonly EnumerableExtractHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(List<string>), typeof(int[])];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public EnumerableExtractHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
