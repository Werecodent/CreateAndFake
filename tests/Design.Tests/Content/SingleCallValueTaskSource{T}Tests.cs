using Werecodent.CreateAndFake.Design.Content;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.TesterTool;

namespace Werecodent.CreateAndFake.Design.Tests.Content;

public sealed class SingleCallValueTaskSource_T_Tests()
    : AutoTester(typeof(SingleCallValueTaskSource<>))
{
    protected override Func<TesterOptions, TesterOptions> OptionConfiguration { get; } =
        opt => opt with { IgnorableExceptions = [typeof(NotSupportedException)] };

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
