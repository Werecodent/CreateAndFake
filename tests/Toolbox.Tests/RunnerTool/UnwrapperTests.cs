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
    internal static Task UnwrapResult_UnwrapsTaskAndIntAsyncEnumerable(IAsyncEnumerable<int> data)
    {
        return UnwrapsTaskAndAsyncEnumerableAsync(data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResult_UnwrapsTaskAndStringAsyncEnumerable(
        IAsyncEnumerable<string> data
    )
    {
        return UnwrapsTaskAndAsyncEnumerableAsync(data);
    }

    private static async Task UnwrapsTaskAndAsyncEnumerableAsync<T>(IAsyncEnumerable<T> data)
    {
        bool iterated = false;

        async IAsyncEnumerable<T> Iterate()
        {
            await foreach (T item in data.WithCancellation(TestContext.Current.CancellationToken))
            {
                yield return item;
            }
            iterated = true;
        }

        object result = await Unwrapper.UnwrapResult(
            () => Task.Run(Iterate, TestContext.Current.CancellationToken),
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
    internal static Task UnwrapResult_UnwrapsTaskAndIntEnumerable(IEnumerable<int> data)
    {
        return UnwrapsTaskAndEnumerableAsync(data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResult_UnwrapsTaskAndStringEnumerable(IEnumerable<string> data)
    {
        return UnwrapsTaskAndEnumerableAsync(data);
    }

    private static async Task UnwrapsTaskAndEnumerableAsync<T>(IEnumerable<T> data)
    {
        bool iterated = false;

        IEnumerable<T> Iterate()
        {
            foreach (T item in data)
            {
                yield return item;
            }
            iterated = true;
        }

        object result = await Unwrapper.UnwrapResult(
            () => Task.Run(Iterate, TestContext.Current.CancellationToken),
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
    internal static Task UnwrapResult_UnwrapsIntTask(int data)
    {
        Task<int> run = Task.Run(() => data, TestContext.Current.CancellationToken);
        return TestUnwrap(() => run, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResult_UnwrapsStringTask(string data)
    {
        Task<string> run = Task.Run(() => data, TestContext.Current.CancellationToken);
        return TestUnwrap(() => run, data);
    }

    [Fact]
    internal static Task UnwrapResult_UnwrapsNullTask()
    {
        Task<object> run = Task.Run(() => (object)null, TestContext.Current.CancellationToken);
        return TestUnwrap(() => run, null);
    }

    [Fact]
    internal static Task UnwrapResult_UnwrapsTask()
    {
        return TestUnwrap(() => Task.CompletedTask, VoidReturn.Instance);
    }

    [Theory, RandomData]
    internal static Task UnwrapResult_UnwrapsIntValueTask(int data)
    {
        ValueTask<int> run = new(data);
        return TestUnwrap(() => run, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResult_UnwrapsStringValueTask(string data)
    {
        ValueTask<string> run = new(Task.FromResult(data));
        return TestUnwrap(() => run, data);
    }

    [Fact]
    internal static Task UnwrapResult_UnwrapsNullValueTask()
    {
        ValueTask<object> run = new(Task.FromResult<object>(null));
        return TestUnwrap(() => run, null);
    }

    [Fact]
    internal static Task UnwrapResult_UnwrapsValueTask()
    {
        return TestUnwrap(() => new ValueTask(Task.CompletedTask), VoidReturn.Instance);
    }

    [Theory, RandomData]
    internal static Task UnwrapResult_UnwrapsString(string data)
    {
        return TestUnwrap(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResult_UnwrapsInt(int data)
    {
        return TestUnwrap(() => data, data);
    }

    [Fact]
    internal static Task UnwrapResult_UnwrapsNull()
    {
        return TestUnwrap(() => null, null);
    }

    [Theory, RandomData]
    internal static Task UnwrapResult_UnwrapsListInt(List<int> data)
    {
        return TestUnwrap(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResult_UnwrapsListString(List<string> data)
    {
        return TestUnwrap(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResult_UnwrapsArrayInt(int[] data)
    {
        return TestUnwrap(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResult_UnwrapsArrayString(string[] data)
    {
        return TestUnwrap(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResult_UnwrapsCollection(ICollection data)
    {
        return TestUnwrap(() => data, data);
    }

    private static Task<AssertChainer<AssertAsyncObject>> TestUnwrap(
        Func<object> call,
        object expectedResult
    )
    {
        return Unwrapper
            .UnwrapResult(call, Tools.Runner.Options, TestContext.Current.CancellationToken)
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .Is(expectedResult);
    }
}
