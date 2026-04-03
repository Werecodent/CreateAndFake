using System.Collections;
using CreateAndFake.Design.Content;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RunnerTool;

public static class UnwrapperTests
{
    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsIntAsyncEnumerable(List<int> data)
    {
        data.Assert()
            .Is(
                await Unwrapper.UnwrapResult(
                    () =>
                        AsyncSeriesHelper.CreateFromAsync(
                            data,
                            data.Count,
                            TestContext.Current.CancellationToken
                        ),
                    Tools.Runner.Options
                )
            );
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsStringAsyncEnumerable(List<string> data)
    {
        data.Assert()
            .Is(
                await Unwrapper.UnwrapResult(
                    () =>
                        AsyncSeriesHelper.CreateFromAsync(
                            data,
                            data.Count,
                            TestContext.Current.CancellationToken
                        ),
                    Tools.Runner.Options
                )
            );
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsIntAsyncEnumerableTask(List<int> data)
    {
        Task<IAsyncEnumerable<int>> run = Task.Run(
            () =>
                AsyncSeriesHelper.CreateFromAsync(
                    data,
                    data.Count,
                    TestContext.Current.CancellationToken
                ),
            TestContext.Current.CancellationToken
        );
        data.Assert().Is(await Unwrapper.UnwrapResult(() => run, Tools.Runner.Options));
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsStringAsyncEnumerableTask(List<string> data)
    {
        Task<IAsyncEnumerable<string>> run = Task.Run(
            () =>
                AsyncSeriesHelper.CreateFromAsync(
                    data,
                    data.Count,
                    TestContext.Current.CancellationToken
                ),
            TestContext.Current.CancellationToken
        );
        data.Assert().Is(await Unwrapper.UnwrapResult(() => run, Tools.Runner.Options));
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsIntTask(int data)
    {
        Task<int> run = Task.Run(() => data, TestContext.Current.CancellationToken);
        data.Assert().Is(await Unwrapper.UnwrapResult(() => run, Tools.Runner.Options));
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsStringTask(string data)
    {
        Task<string> run = Task.Run(() => data, TestContext.Current.CancellationToken);
        data.Assert().Is(await Unwrapper.UnwrapResult(() => run, Tools.Runner.Options));
    }

    [Fact]
    internal static async Task UnwrapResult_UnwrapsNullTask()
    {
        Task<object> run = Task.Run(() => (object)null, TestContext.Current.CancellationToken);
        (await Unwrapper.UnwrapResult(() => run, Tools.Runner.Options)).Assert().IsNull();
    }

    [Fact]
    internal static async Task UnwrapResult_UnwrapsTask()
    {
        await (await Unwrapper.UnwrapResult(() => Task.CompletedTask, Tools.Runner.Options))
            .Assert()
            .IsAsync(VoidReturn.Instance, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsIntValueTask(int data)
    {
        ValueTask<int> run = new(Task.FromResult(data));
        data.Assert().Is(await Unwrapper.UnwrapResult(() => run, Tools.Runner.Options));
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsStringValueTask(string data)
    {
        ValueTask<string> run = new(Task.FromResult(data));
        data.Assert().Is(await Unwrapper.UnwrapResult(() => run, Tools.Runner.Options));
    }

    [Fact]
    internal static async Task UnwrapResult_UnwrapsNullValueTask()
    {
        ValueTask<object> run = new(Task.FromResult<object>(null));
        (await Unwrapper.UnwrapResult(() => run, Tools.Runner.Options)).Assert().IsNull();
    }

    [Fact]
    internal static async Task UnwrapResult_UnwrapsValueTask()
    {
        await (
            await Unwrapper.UnwrapResult(
                () => new ValueTask(Task.CompletedTask),
                Tools.Runner.Options
            )
        )
            .Assert()
            .IsAsync(VoidReturn.Instance, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsString(string data)
    {
        data.Assert().Is(await Unwrapper.UnwrapResult(() => data, Tools.Runner.Options));
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsInt(int data)
    {
        data.Assert().Is(await Unwrapper.UnwrapResult(() => data, Tools.Runner.Options));
    }

    [Fact]
    internal static async Task UnwrapResult_UnwrapsNull()
    {
        (await Unwrapper.UnwrapResult(() => null, Tools.Runner.Options)).Assert().IsNull();
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsListInt(List<int> data)
    {
        data.Assert().Is(await Unwrapper.UnwrapResult(() => data, Tools.Runner.Options));
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsListString(List<string> data)
    {
        data.Assert().Is(await Unwrapper.UnwrapResult(() => data, Tools.Runner.Options));
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsArrayInt(int[] data)
    {
        data.Assert().Is(await Unwrapper.UnwrapResult(() => data, Tools.Runner.Options));
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsArrayString(string[] data)
    {
        data.Assert().Is(await Unwrapper.UnwrapResult(() => data, Tools.Runner.Options));
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsEnumerableInt(List<int> data)
    {
        data.Assert().Is(await Unwrapper.UnwrapResult(() => Yielded(data), Tools.Runner.Options));
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsEnumerableString(List<string> data)
    {
        data.Assert().Is(await Unwrapper.UnwrapResult(() => Yielded(data), Tools.Runner.Options));
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsCollection(ICollection data)
    {
        data.Assert().Is(await Unwrapper.UnwrapResult(() => data, Tools.Runner.Options));
    }

    private static IEnumerable<T> Yielded<T>(IEnumerable<T> data)
    {
        foreach (T item in data)
        {
            yield return item;
        }
    }
}
