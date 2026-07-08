using System.Collections;
using CreateAndFake.Fluent.AssertAsyncCalls;
using CreateAndFake.Fluent.Chaining;
using CreateAndFake.RunnerTool;

namespace CreateAndFake.Tests.RunnerTool;

public static class UnwrapperTests
{
    [Fact]
    internal static Task Unwrapper_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(Unwrapper),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task Unwrapper_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(Unwrapper),
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsTaskAndIntAsyncEnumerable(
        IAsyncEnumerable<int> data
    )
    {
        return UnwrapsTaskAndAsyncEnumerableAsync(data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsTaskAndStringAsyncEnumerable(
        IAsyncEnumerable<string> data
    )
    {
        return UnwrapsTaskAndAsyncEnumerableAsync(data);
    }

    private static async Task UnwrapsTaskAndAsyncEnumerableAsync<T>(IAsyncEnumerable<T> data)
    {
        bool iterated = false;

        async IAsyncEnumerable<T> iterate()
        {
            await foreach (T item in data.WithCancellation(TestContext.Current.CancellationToken))
            {
                yield return item;
            }
            iterated = true;
        }

        object result = await Unwrapper.UnwrapResultAsync(
            () => Task.Run(iterate, TestContext.Current.CancellationToken),
            Tools.Runner.Options,
            TestContext.Current.CancellationToken
        );

        await iterated
            .Assert()
            .Is(true)
            .Also(result)
            .IsAsync(data, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsTaskAndIntEnumerable(IEnumerable<int> data)
    {
        return UnwrapsTaskAndEnumerableAsync(data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsTaskAndStringEnumerable(IEnumerable<string> data)
    {
        return UnwrapsTaskAndEnumerableAsync(data);
    }

    private static async Task UnwrapsTaskAndEnumerableAsync<T>(IEnumerable<T> data)
    {
        bool iterated = false;

        IEnumerable<T> iterate()
        {
            foreach (T item in data)
            {
                yield return item;
            }
            iterated = true;
        }

        object result = await Unwrapper.UnwrapResultAsync(
            () => Task.Run(iterate, TestContext.Current.CancellationToken),
            Tools.Runner.Options,
            TestContext.Current.CancellationToken
        );

        await iterated
            .Assert()
            .Is(true)
            .Also(result)
            .IsAsync(data, TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsIntTask(int data)
    {
        Task<int> run = Task.Run(() => data, TestContext.Current.CancellationToken);
        return TestUnwrap(() => run, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsStringTask(string data)
    {
        Task<string> run = Task.Run(() => data, TestContext.Current.CancellationToken);
        return TestUnwrap(() => run, data);
    }

    [Fact]
    internal static Task UnwrapResultAsync_UnwrapsNullTask()
    {
        Task<object> run = Task.Run(() => (object)null, TestContext.Current.CancellationToken);
        return TestUnwrap(() => run, null);
    }

    [Fact]
    internal static Task UnwrapResultAsync_UnwrapsTask()
    {
        return TestUnwrap(() => Task.CompletedTask, VoidReturn.Instance);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsIntValueTask(int data)
    {
        ValueTask<int> run = new(data);
        return TestUnwrap(() => run, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsStringValueTask(string data)
    {
        ValueTask<string> run = new(Task.FromResult(data));
        return TestUnwrap(() => run, data);
    }

    [Fact]
    internal static Task UnwrapResultAsync_UnwrapsNullValueTask()
    {
        ValueTask<object> run = new(Task.FromResult<object>(null));
        return TestUnwrap(() => run, null);
    }

    [Fact]
    internal static Task UnwrapResultAsync_UnwrapsValueTask()
    {
        return TestUnwrap(() => new ValueTask(Task.CompletedTask), VoidReturn.Instance);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsString(string data)
    {
        return TestUnwrap(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsInt(int data)
    {
        return TestUnwrap(() => data, data);
    }

    [Fact]
    internal static Task UnwrapResultAsync_UnwrapsNull()
    {
        return TestUnwrap(() => null, null);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsListInt(List<int> data)
    {
        return TestUnwrap(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsListString(List<string> data)
    {
        return TestUnwrap(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsArrayInt(int[] data)
    {
        return TestUnwrap(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsArrayString(string[] data)
    {
        return TestUnwrap(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsCollection(ICollection data)
    {
        return TestUnwrap(() => data, data);
    }

    private static Task<AssertChainer<AssertAsyncObject>> TestUnwrap(
        Func<object> call,
        object expectedResult
    )
    {
        return Unwrapper
            .UnwrapResultAsync(call, Tools.Runner.Options, TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .Is(expectedResult);
    }
}
