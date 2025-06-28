using CreateAndFake.ExtractorTool.Hints;

namespace CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class EnumerableExtractHintTests : ExtractHintTestBase<EnumerableExtractHint>
{
    private static readonly EnumerableExtractHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(List<string>), typeof(int[])];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public EnumerableExtractHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
