using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public sealed class SingleCallValueTaskSourceTests : AutoTester<SingleCallValueTaskSource>
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(NotSupportedException)] };

    [Theory, RandomData]
    internal static async Task GetResult_Prevents2ndAwait(
        SingleCallValueTaskSource source,
        short token
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        ValueTask task = new(source, token);
        await task.Assert().ThrowsNoAsync<Exception>(canceler);
        await task.Assert().ThrowsAsync<ValueTaskRepeatedAccessException>(canceler);
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
