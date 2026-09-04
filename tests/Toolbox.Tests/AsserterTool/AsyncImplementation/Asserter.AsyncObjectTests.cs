using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.AsserterTool.AsyncImplementation;

public sealed class AsserterAsyncObjectTests
{
    private readonly Asserter _testInstance = new(Tools.Asserter.Options);

    [Theory, RandomData]
    internal Task IsAsync_NoThrowWithSame(object item, [Copy] object item2)
    {
        return _testInstance
            .IsAsync(item, item2, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task IsAsync_ThrowsWithVariant(object item, object item2)
    {
        return _testInstance
            .IsAsync(item, item2, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task IsNotAsync_NoThrowWithVariant(object item, object item2)
    {
        return _testInstance
            .IsNotAsync(item, item2, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<Exception>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task IsNotAsync_ThrowsWithSame(object item, [Copy] object item2)
    {
        return _testInstance
            .IsNotAsync(item, item2, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task AreUniqueAsync_ThrowsWithSame(AsyncDataSample item, [Copy] AsyncDataSample item2)
    {
        return _testInstance
            .AreUniqueAsync(item, item2, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }
}
