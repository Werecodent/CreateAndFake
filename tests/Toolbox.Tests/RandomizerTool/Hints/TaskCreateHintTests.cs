using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class TaskCreateHintTests : CreateHintTestBase<TaskCreateHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(Task<DataHolderSample>),
        typeof(Task<object>),
        typeof(Task<string>),
        typeof(Task<int>),
        typeof(Task<bool>),
        typeof(TaskCompletionSource<string>),
        typeof(Task),
    ];

    public TaskCreateHintTests()
        : base(_ValidTypes) { }

    [Fact]
    internal static void TryToCreate_TaskTypingCorrect()
    {
        Tools.Randomizer.Create<Task<object>>().GetType().Assert().Is(typeof(Task<object>));
    }
}
