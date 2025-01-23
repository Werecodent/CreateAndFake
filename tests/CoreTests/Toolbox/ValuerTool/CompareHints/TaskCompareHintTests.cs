using System.Collections;
using CreateAndFake.Toolbox.ValuerTool.CompareHints;
using CreateAndFakeTests.TestSamples;

#pragma warning disable CA1849 // Task await synchronously blocks: For testing.

namespace CreateAndFakeTests.Toolbox.ValuerTool.CompareHints;

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
        BuildTask(ex)
            .Assert()
            .IsNot(BuildTask(Tools.Mutator.Variant(ex)))
            .And
            .IsNot(BuildTask(null));
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
