using CreateAndFake.Toolbox.DuplicatorTool;
using CreateAndFake.Toolbox.DuplicatorTool.CopyHints;
using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.Toolbox.DuplicatorTool.CopyHints;

public sealed class TaskCopyHintTests : CopyHintTestBase<TaskCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(Task<DataHolderSample>),
        typeof(Task<object>),
        typeof(Task<string>),
        typeof(Task<int>),
        typeof(Task<bool>)
    ];

    private static readonly Type[] _InvalidTypes = [typeof(object)];

    public TaskCopyHintTests() : base(_ValidTypes, _InvalidTypes) { }

    [Fact]
    internal void TryCopy_NonGenericTaskFalse()
    {
        TestInstance.TryCopy(new Task(() => { }), CreateChainer()).Assert().Is(CopyHintResult.None);
    }
}
