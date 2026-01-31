using CreateAndFake.ExtractorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class DictionaryExtractHintTests : ExtractHintTestBase<DictionaryExtractHint>
{
    private static readonly DictionaryExtractHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(Dictionary<int, string>)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public DictionaryExtractHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
