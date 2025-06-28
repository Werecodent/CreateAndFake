using CreateAndFake.ExtractorTool.Hints;

namespace CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class EndingExtractHintTests : ExtractHintTestBase<EndingExtractHint>
{
    private static readonly EndingExtractHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [typeof(int), typeof(string), typeof(Type)];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public EndingExtractHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
