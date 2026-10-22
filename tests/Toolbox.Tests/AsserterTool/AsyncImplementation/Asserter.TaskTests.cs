using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Tests.AsserterTool.AsyncImplementation;

public sealed class AsserterTaskTests
{
    private readonly Asserter _testInstance = new(Tools.Asserter.Options);

    [Theory, RandomData]
    internal Task HasResultAsync_ReturnsResult(int value)
    {
        return _testInstance
            .HasResultAsync(Task.FromResult(value), TestContext.Current.CancellationToken)
            .Assert()
            .IsAsync(Task.FromResult(value), TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task ThrowsNoAsync_IgnoresDifferentException(InvalidOperationException error)
    {
        return _testInstance.ThrowsNoAsync<ArgumentException>(
            Task.FromException(error),
            TestContext.Current.CancellationToken
        );
    }
}
