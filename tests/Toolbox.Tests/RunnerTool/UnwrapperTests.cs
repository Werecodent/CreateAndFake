using System.Collections;
using CreateAndFake.Design.Content;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RunnerTool;

public static class UnwrapperTests
{
    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsIntAsyncEnumerable(List<int> data)
    {
        (await Unwrapper.UnwrapResult(() => AsyncEnumHelper.CreateFrom(data))).Assert().Is(data);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsStringAsyncEnumerable(List<string> data)
    {
        (await Unwrapper.UnwrapResult(() => AsyncEnumHelper.CreateFrom(data))).Assert().Is(data);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsIntAsyncEnumerableTask(List<int> data)
    {
        Task<IAsyncEnumerable<int>> run = Task.Run(() => AsyncEnumHelper.CreateFrom(data));
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(data);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsStringAsyncEnumerableTask(List<string> data)
    {
        Task<IAsyncEnumerable<string>> run = Task.Run(() => AsyncEnumHelper.CreateFrom(data));
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(data);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsIntTask(int data)
    {
        Task<int> run = Task.Run(() => data);
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(data);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsStringTask(string data)
    {
        Task<string> run = Task.Run(() => data);
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(data);
    }

    [Fact]
    internal static async Task UnwrapResult_UnwrapsNullTask()
    {
        Task<object> run = Task.Run(() => (object)null);
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(null);
    }

    [Fact]
    internal static async Task UnwrapResult_UnwrapsTask()
    {
        (await Unwrapper.UnwrapResult(() => Task.CompletedTask)).Assert().Is(VoidReturn.Instance);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsIntValueTask(int data)
    {
        ValueTask<int> run = new(Task.FromResult(data));
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(data);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsStringValueTask(string data)
    {
        ValueTask<string> run = new(Task.FromResult(data));
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(data);
    }

    [Fact]
    internal static async Task UnwrapResult_UnwrapsNullValueTask()
    {
        ValueTask<object> run = new(Task.FromResult<object>(null));
        (await Unwrapper.UnwrapResult(() => run)).Assert().Is(null);
    }

    [Fact]
    internal static async Task UnwrapResult_UnwrapsValueTask()
    {
        (await Unwrapper.UnwrapResult(() => new ValueTask(Task.CompletedTask)))
            .Assert()
            .Is(VoidReturn.Instance);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsString(string data)
    {
        (await Unwrapper.UnwrapResult(() => data)).Assert().Is(data);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsInt(int data)
    {
        (await Unwrapper.UnwrapResult(() => data)).Assert().Is(data);
    }

    [Fact]
    internal static async Task UnwrapResult_UnwrapsNull()
    {
        (await Unwrapper.UnwrapResult(() => null)).Assert().Is(null);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsListInt(List<int> data)
    {
        (await Unwrapper.UnwrapResult(() => data)).Assert().Is(data);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsListString(List<string> data)
    {
        (await Unwrapper.UnwrapResult(() => data)).Assert().Is(data);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsArrayInt(int[] data)
    {
        (await Unwrapper.UnwrapResult(() => data)).Assert().Is(data);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsArrayString(string[] data)
    {
        (await Unwrapper.UnwrapResult(() => data)).Assert().Is(data);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsEnumerableInt(List<int> data)
    {
        (await Unwrapper.UnwrapResult(() => Yielded(data))).Assert().Is(data);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsEnumerableString(List<string> data)
    {
        (await Unwrapper.UnwrapResult(() => Yielded(data))).Assert().Is(data);
    }

    [Theory, RandomData]
    internal static async Task UnwrapResult_UnwrapsCollection(ICollection data)
    {
        (await Unwrapper.UnwrapResult(() => data)).Assert().Is(data);
    }

    private static IEnumerable<T> Yielded<T>(IEnumerable<T> data)
    {
        foreach (T item in data)
        {
            yield return item;
        }
    }
}
