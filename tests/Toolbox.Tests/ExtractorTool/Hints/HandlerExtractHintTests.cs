using CreateAndFake.ExtractorTool.Hints;

namespace CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class HandlerExtractHintTests : ExtractHintTestBase<HandlerExtractHint>
{
    private static readonly HandlerExtractHint _TestInstance = new();

    private static readonly Type[] _ValidTypes = [.. _TestInstance.SupportedTypes];

    private static readonly Type[] _InvalidTypes = [];

    public HandlerExtractHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
