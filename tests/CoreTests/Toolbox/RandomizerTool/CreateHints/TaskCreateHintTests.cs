using CreateAndFake.Toolbox.RandomizerTool.CreateHints;
using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.Toolbox.RandomizerTool.CreateHints;

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
        typeof(Task)
    ];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public TaskCreateHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }
}
