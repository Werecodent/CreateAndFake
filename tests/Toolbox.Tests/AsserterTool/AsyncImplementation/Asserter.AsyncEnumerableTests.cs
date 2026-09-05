using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Content;

namespace Werecodent.CreateAndFake.Tests.AsserterTool.AsyncImplementation;

public sealed class AsserterAsyncEnumerableTests
{
    private readonly Asserter _testInstance = new(Tools.Asserter.Options);

    [Theory, RandomData]
    internal Task FailAsync_Throws(IAsyncEnumerable<object> series)
    {
        return _testInstance
            .FailAsync(series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task IsEmptyAsync_NoThrowWithNone([Size(0)] IAsyncEnumerable<object> series)
    {
        return _testInstance
            .IsEmptyAsync(series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task IsEmptyAsync_ThrowWithSome([Size(1)] IAsyncEnumerable<object> series)
    {
        return _testInstance
            .IsEmptyAsync(series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task IsNotEmptyAsync_ThrowWithNone([Size(0)] IAsyncEnumerable<object> series)
    {
        return _testInstance
            .IsNotEmptyAsync(series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal Task IsNotEmptyAsync_NoThrowWithSome([Size(1)] IAsyncEnumerable<object> series)
    {
        return _testInstance
            .IsNotEmptyAsync(series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal async Task HasCountAsync_ThrowUnlessSame([Size(1)] IAsyncEnumerable<object> series)
    {
        await _testInstance
            .HasCountAsync(0, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);

        await _testInstance
            .HasCountAsync(1, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<AssertException>(TestContext.Current.CancellationToken);

        await _testInstance
            .HasCountAsync(2, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal async Task HasCountLessThanAsync_ThrowUnlessLess(
        [Size(1)] IAsyncEnumerable<object> series
    )
    {
        await _testInstance
            .HasCountLessThanAsync(0, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);

        await _testInstance
            .HasCountLessThanAsync(1, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);

        await _testInstance
            .HasCountLessThanAsync(2, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal async Task HasCountLessOrExactlyAsync_ThrowWhenGreater(
        [Size(1)] IAsyncEnumerable<object> series
    )
    {
        await _testInstance
            .HasCountLessOrExactlyAsync(0, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);

        await _testInstance
            .HasCountLessOrExactlyAsync(1, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<AssertException>(TestContext.Current.CancellationToken);

        await _testInstance
            .HasCountLessOrExactlyAsync(2, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal async Task HasCountMoreThanAsync_ThrowUnlessGreater(
        [Size(1)] IAsyncEnumerable<object> series
    )
    {
        await _testInstance
            .HasCountMoreThanAsync(0, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<AssertException>(TestContext.Current.CancellationToken);

        await _testInstance
            .HasCountMoreThanAsync(1, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);

        await _testInstance
            .HasCountMoreThanAsync(2, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal async Task HasCountMoreOrExactlyAsync_ThrowUnlessGreater(
        [Size(1)] IAsyncEnumerable<object> series
    )
    {
        await _testInstance
            .HasCountMoreOrExactlyAsync(0, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<AssertException>(TestContext.Current.CancellationToken);

        await _testInstance
            .HasCountMoreOrExactlyAsync(1, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<AssertException>(TestContext.Current.CancellationToken);

        await _testInstance
            .HasCountMoreOrExactlyAsync(2, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal async Task ContainsAsync_ThrowWithoutContent([Size(1)] IAsyncEnumerable<object> series)
    {
        object item = (
            await AsyncSeriesHelper.ToListAsync(series, 2, TestContext.Current.CancellationToken)
        ).Single();

        await _testInstance
            .ContainsAsync(item, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<AssertException>(TestContext.Current.CancellationToken);

        await _testInstance
            .ContainsAsync(item.Tools().Variant(), series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal async Task ContainsNotAsync_ThrowWithContent([Size(1)] IAsyncEnumerable<object> series)
    {
        object item = (
            await AsyncSeriesHelper.ToListAsync(series, 2, TestContext.Current.CancellationToken)
        ).Single();

        await _testInstance
            .ContainsNotAsync(item, series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);

        await _testInstance
            .ContainsNotAsync(item.Tools().Variant(), series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsNoAsync<AssertException>(TestContext.Current.CancellationToken);
    }
}
