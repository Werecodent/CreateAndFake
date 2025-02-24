using System.Collections;
using CreateAndFake.Tests.TestSamples;
using CreateAndFake.ValuerTool;
using CreateAndFake.ValuerTool.CompareHints;

#pragma warning disable CA1849 // Task await synchronously blocks: For testing.

namespace CreateAndFake.Tests.ValuerTool.CompareHints;

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
        typeof(Task)
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(IEnumerable),
        typeof(string),
        typeof(int)
    ];

    public TaskCompareHintTests() : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void Compare_NonGenericTasksCompareByException(Exception ex)
    {
        Task taskA = BuildTask(ex);
        Task taskB = BuildTask(ex);
        Task taskC = BuildTask(Tools.Mutator.Variant(ex));
        Task taskD = BuildTask(null);
        ValuerChainer chainer = CreateChainer();
        TestInstance.TryCompare(taskA, taskB, chainer).Assert().Is(new DifferenceHintResult([]));
        TestInstance.TryCompare(taskA, taskC, chainer).Assert().IsNot(new DifferenceHintResult([]));
        TestInstance.TryCompare(taskA, taskD, chainer).Assert().IsNot(new DifferenceHintResult([]));
    }

    [Fact]
    internal void Compare_NonGenericTasksCompareByStatus()
    {
        Task taskA = BuildTask(null);
        Task taskB = BuildTask(null);
        ValuerChainer chainer = CreateChainer();
        TestInstance.TryCompare(taskA, taskB, chainer).Assert().Is(new DifferenceHintResult([]));
    }

    private static Task BuildTask(Exception ex)
    {
        Task task = new(() =>
        {
            if (ex != null)
            {
                throw ex;
            }
        });
        try
        {
            task.Start();
            task.Wait();
        }
        catch (AggregateException)
        {
            // Throw intentional.
        }
        return task;
    }
}

#pragma warning restore CA1849 // Task await synchronously blocks
