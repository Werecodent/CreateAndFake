using CreateAndFake.DuplicatorTool.Engine;
using CreateAndFake.DuplicatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class TaskCopyHintTests : CopyHintTestBase<TaskCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(Task<DataHolderSample>),
        typeof(Task<object>),
        typeof(Task<string>),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(DataHolderSample)];

    public TaskCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    [Fact]
    internal async Task TryCopy_NonGenericTaskSupport()
    {
        Task task = Task.Run(() => { }, TestContext.Current.CancellationToken);
        await task;
        await TestInstance
            .TryCopy(task, CreateChainer())
            .Assert()
            .IsAsync(new CopyHintResult(Task.CompletedTask), TestContext.Current.CancellationToken);
    }

    [Fact]
    internal Task TryCopy_CompletedTaskSupport()
    {
        return TestInstance
            .TryCopy(Task.CompletedTask, CreateChainer())
            .Assert()
            .IsAsync(new CopyHintResult(Task.CompletedTask), TestContext.Current.CancellationToken);
    }
}
