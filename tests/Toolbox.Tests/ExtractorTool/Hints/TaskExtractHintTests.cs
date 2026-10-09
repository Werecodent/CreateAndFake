using Werecodent.CreateAndFake.ExtractorTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ExtractorTool.Hints;

public sealed class TaskExtractHintTests : ExtractHintTestBase<TaskExtractHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(Task),
        typeof(Task<int>),
        typeof(Task<string>),
    ];

    public TaskExtractHintTests()
        : base(_ValidTypes) { }
}
