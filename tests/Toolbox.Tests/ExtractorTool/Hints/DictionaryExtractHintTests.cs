using CreateAndFake.ExtractorTool.Hints;

namespace CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class DictionaryExtractHintTests : ExtractHintTestBase<DictionaryExtractHint>
{
    private static readonly DictionaryExtractHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(Dictionary<int, string>)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public DictionaryExtractHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
