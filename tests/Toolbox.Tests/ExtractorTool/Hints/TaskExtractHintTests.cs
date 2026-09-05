using Werecodent.CreateAndFake.ExtractorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class TaskExtractHintTests : ExtractHintTestBase<TaskExtractHint>
{
    private static readonly TaskExtractHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(Task),
        typeof(Task<int>),
        typeof(Task<string>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public TaskExtractHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
