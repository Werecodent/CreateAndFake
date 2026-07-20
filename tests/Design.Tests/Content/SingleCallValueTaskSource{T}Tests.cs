using CreateAndFake.Design.Content;
using CreateAndFake.Design.Exceptions;

namespace CreateAndFake.Design.Tests.Content;

public static class SingleCallValueTaskSource_T_Tests
{
    [Fact]
    internal static Task SingleCallValueTaskSource_T_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(SingleCallValueTaskSource<>),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(NotSupportedException)] }
        );
    }

    [Fact]
    internal static Task SingleCallValueTaskSource_T_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(SingleCallValueTaskSource<>),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(NotSupportedException)] }
        );
    }

    [Theory, RandomData]
    internal static async Task GetResult_Prevents2ndAwait(
        SingleCallValueTaskSource<string> source,
        short token
    )
    {
        ValueTask<string> task = new(source, token);
        await task.Assert().HasResultAsync(TestContext.Current.CancellationToken);
        await task.Assert()
            .ThrowsAsync<ValueTaskRepeatedAccessException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task ExtractFrom_ValidWithSource(
        SingleCallValueTaskSource<int> source,
        short token
    )
    {
        ValueTask<int> task = new(source, token);
        SingleCallValueTaskSource<int>.ExtractFrom(task).Assert().Is(source);
        SingleCallValueTaskSource<int>.ExtractTokenFrom(task).Assert().Is(token);
        return task.Assert().ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task ExtractFrom_NullWithoutSource(string data)
    {
        ValueTask<string> task = new(data);
        SingleCallValueTaskSource<string>.ExtractFrom(task).Assert().IsNull();
        return task.Assert().HasResultAsync(TestContext.Current.CancellationToken);
    }
}
