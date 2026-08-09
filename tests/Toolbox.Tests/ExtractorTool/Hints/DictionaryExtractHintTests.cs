using Werecodent.CreateAndFake.ExtractorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class DictionaryExtractHintTests : ExtractHintTestBase<DictionaryExtractHint>
{
    private static readonly DictionaryExtractHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(Dictionary<int, string>)];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public DictionaryExtractHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
