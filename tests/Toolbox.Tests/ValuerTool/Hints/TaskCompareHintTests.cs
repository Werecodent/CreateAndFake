using System.Collections;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class TaskCompareHintTests : CompareHintTestBase<TaskCompareHint>
{
    private static readonly TaskCompareHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(Task<DataHolderSample>),
        typeof(Task<object>),
        typeof(Task<string>),
        typeof(Task<int>),
        typeof(Task<bool>),
        typeof(Task),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(IEnumerable),
        typeof(string),
        typeof(int),
    ];

    public TaskCompareHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void Compare_NonGenericTasksCompareByException(Exception ex)
    {
        Task taskA = Task.FromException(ex);
        Task taskB = Task.FromException(ex);
        Task taskC = Task.FromException(Tools.Mutator.Variant(ex));
        Task taskD = Task.CompletedTask;
        IValuerChainer chainer = CreateChainer();
        TestInstance.TryToCompare(taskA, taskB, chainer).Assert().Is(new DifferenceHintResult([]));
        TestInstance
            .TryToCompare(taskA, taskC, chainer)
            .Assert()
            .IsNot(new DifferenceHintResult([]));
        TestInstance
            .TryToCompare(taskA, taskD, chainer)
            .Assert()
            .IsNot(new DifferenceHintResult([]));
    }

    [Fact]
    internal void Compare_NonGenericTasksCompareByStatus()
    {
        Task taskA = Task.CompletedTask;
        Task taskB = Task.CompletedTask;
        IValuerChainer chainer = CreateChainer();
        TestInstance.TryToCompare(taskA, taskB, chainer).Assert().Is(new DifferenceHintResult([]));
    }
}
