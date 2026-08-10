using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Tests.AsserterTool.AsyncImplementation;

public sealed class AsserterTaskTests
{
    private readonly Asserter _testInstance = new(Tools.Asserter.Options);

    [Theory, RandomData]
    internal Task HasResultAsync_Throws(IAsyncEnumerable<object> series)
    {
        return _testInstance
            .FailAsync(series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }
}
