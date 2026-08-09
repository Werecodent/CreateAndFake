using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public static class SingleCallValueTaskSourceTests
{
    [Fact]
    internal static Task SingleCallValueTaskSource_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            typeof(SingleCallValueTaskSource),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(NotSupportedException)] }
        );
    }

    [Fact]
    internal static Task SingleCallValueTaskSource_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            typeof(SingleCallValueTaskSource),
            TestContext.Current.CancellationToken,
            opt => opt with { IgnorableExceptions = [typeof(NotSupportedException)] }
        );
    }

    [Fact]
    internal static void SingleCallValueTaskSource_VerifyValueEquality()
    {
        Tools.Tester.VerifyValueEquality<SingleCallValueTaskSource>();
    }

    [Theory, RandomData]
    internal static async Task GetResult_Prevents2ndAwait(
        SingleCallValueTaskSource source,
        short token
    )
    {
        ValueTask task = new(source, token);
        await task.Assert().ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken);
        await task.Assert()
            .ThrowsAsync<ValueTaskRepeatedAccessException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task ExtractFrom_ValidWithSource(SingleCallValueTaskSource source, short token)
    {
        ValueTask task = new(source, token);
        SingleCallValueTaskSource.ExtractFrom(task).Assert().Is(source);
        SingleCallValueTaskSource.ExtractTokenFrom(task).Assert().Is(token);
        return task.Assert().ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static Task ExtractFrom_NullWithoutSource()
    {
        ValueTask task = new(Task.CompletedTask);
        SingleCallValueTaskSource.ExtractFrom(task).Assert().IsNull();
        return task.Assert().ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken);
    }
}
