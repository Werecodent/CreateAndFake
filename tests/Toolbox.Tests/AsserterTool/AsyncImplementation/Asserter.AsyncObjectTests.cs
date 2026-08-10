using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Tests.AsserterTool.AsyncImplementation;

public sealed class AsserterAsyncObjectTests
{
    private readonly Asserter _testInstance = new(Tools.Asserter.Options);

    [Theory, RandomData]
    internal Task IsAsync_Throws(object item, object item2)
    {
        return _testInstance
            .IsAsync(item, item2, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }
}
