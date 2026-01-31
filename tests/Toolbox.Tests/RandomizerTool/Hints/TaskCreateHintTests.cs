using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class TaskCreateHintTests : CreateHintTestBase<TaskCreateHint>
{
    private static readonly TaskCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(Task<DataHolderSample>),
        typeof(Task<object>),
        typeof(Task<string>),
        typeof(Task<int>),
        typeof(Task<bool>),
        typeof(Task),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public TaskCreateHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
