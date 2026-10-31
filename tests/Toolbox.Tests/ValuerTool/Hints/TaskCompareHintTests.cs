using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.ValuerTool.Engine;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class TaskCompareHintTests : CompareHintTestBase<TaskCompareHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(Task<DataHolderSample>),
        typeof(Task<object>),
        typeof(Task<string>),
        typeof(Task<int>),
        typeof(Task<bool>),
        typeof(Task),
    ];

    public TaskCompareHintTests()
        : base(_ValidTypes) { }

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

    [Fact]
    internal Task GetHashCodeAsync_TimesOut()
    {
        return TestInstance
            .TryToAsyncGetHashCode(
                Task.Delay(500, TestContext.Current.CancellationToken),
                CreateChainer(Tools.Valuer.Options with { AsyncTimeout = TimeSpan.Zero }),
                TestContext.Current.CancellationToken
            )
            .Data.Assert()
            .ThrowsAsync<EngineException>(TestContext.Current.CancellationToken);
    }
}
