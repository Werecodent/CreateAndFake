using System.Collections;
using Werecodent.CreateAndFake.Fluent.AssertAsyncCalls;
using Werecodent.CreateAndFake.Fluent.Chaining;
using Werecodent.CreateAndFake.RunnerTool;

namespace Werecodent.CreateAndFake.Tests.RunnerTool;

#pragma warning disable IDE0059

public static class UnwrapperTests
{
    [Fact]
    internal static Task Unwrapper_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(Unwrapper),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ArgumentException)] }
        );
    }

    [Fact]
    internal static Task Unwrapper_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(Unwrapper),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(ArgumentException)] }
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

        async IAsyncEnumerable<T> iterateAsync()
        {
            await foreach (T item in data.WithCancellation(TestContext.Current.CancellationToken))
            {
                yield return item;
            }
            iterated = true;
        }

        IAsyncEnumerable<T> series = iterateAsync();
        iterated.Assert().Is(false);

        object result = await Unwrapper.UnwrapResultAsync(
            series,
            typeof(IAsyncEnumerable<T>),
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

        IEnumerable<T> series = iterate();
        iterated.Assert().Is(false);

        object result = await Unwrapper.UnwrapResultAsync(
            series,
            typeof(IEnumerable<T>),
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
    internal static Task UnwrapResultAsync_UnwrapsIntTask(Task<int> data)
    {
        return TestUnwrapAsync(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsStringTask(Task<string> data)
    {
        return TestUnwrapAsync(() => data, data);
    }

    [Fact]
    internal static Task UnwrapResultAsync_UnwrapsNullTask()
    {
        Task<object> run = Task.Run(() => (object)null, TestContext.Current.CancellationToken);
        return TestUnwrapAsync(() => run, run);
    }

    [Fact]
    internal static Task UnwrapResultAsync_UnwrapsTask()
    {
        return TestUnwrapAsync(() => Task.CompletedTask, Task.CompletedTask);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsIntValueTask(int data)
    {
        ValueTask<int> run = new(data);
        return TestUnwrapAsync(() => run, Task.FromResult(data));
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsStringValueTask(string data)
    {
        ValueTask<string> run = new(Task.FromResult(data));
        return TestUnwrapAsync(() => run, Task.FromResult(data));
    }

    [Fact]
    internal static Task UnwrapResultAsync_UnwrapsNullValueTask()
    {
        ValueTask<object> run = new(Task.FromResult<object>(null));
        return TestUnwrapAsync(() => run, Task.FromResult((object)null));
    }

    [Fact]
    internal static Task UnwrapResultAsync_UnwrapsNullValueTaskWithType()
    {
        ValueTask<string> run = new(Task.FromResult<string>(null));
        return TestUnwrapAsync(() => run, Task.FromResult((string)null));
    }

    [Fact]
    internal static Task UnwrapResultAsync_UnwrapsValueTask()
    {
        return TestUnwrapAsync(() => new ValueTask(Task.CompletedTask), Task.CompletedTask);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsString(string data)
    {
        return TestUnwrapAsync(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsInt(int data)
    {
        return TestUnwrapAsync(() => data, data);
    }

    [Fact]
    internal static Task UnwrapResultAsync_UnwrapsNull()
    {
        return TestUnwrapAsync<object>(() => null, null);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsListInt(List<int> data)
    {
        return TestUnwrapAsync(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsListString(List<string> data)
    {
        return TestUnwrapAsync(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsArrayInt(int[] data)
    {
        return TestUnwrapAsync(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsArrayString(string[] data)
    {
        return TestUnwrapAsync(() => data, data);
    }

    [Theory, RandomData]
    internal static Task UnwrapResultAsync_UnwrapsCollection(ICollection data)
    {
        return TestUnwrapAsync(() => data, data);
    }

    private static Task<AssertChainer<AssertAsyncObject>> TestUnwrapAsync<T>(
        Func<T> call,
        object expectedResult
    )
    {
        return Unwrapper
            .UnwrapResultAsync(
                call(),
                typeof(T),
                Tools.Runner.Options,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .HasResultAsync(TestContext.Current.CancellationToken)
            .That()
            .Is(expectedResult);
    }
}

#pragma warning restore
