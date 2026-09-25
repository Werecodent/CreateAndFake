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
    internal async Task FailAsync_ModAppliesOnce(IAsyncEnumerable<object> series)
    {
        int appliedCount = 0;

        await _testInstance
            .FailAsync(
                series,
                TestContext.Current.CancellationToken,
                opt =>
                {
                    appliedCount++;
                    return opt;
                }
            )
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);

        appliedCount.Assert().Is(1);
    }

    [Theory, RandomData]
    internal Task DebugAsync_DoesNotThrow(IAsyncEnumerable<object> series)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        return _testInstance
            .DebugAsync(series, canceler, opt => opt with { DebugAssertsFail = false })
            .Assert()
            .ThrowsNoAsync<Exception>(canceler);
    }

    [Theory, RandomData]
    internal Task DebugAsync_ThrowsWhenConfigured(IAsyncEnumerable<object> series)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        return _testInstance
            .DebugAsync(series, canceler, opt => opt with { DebugAssertsFail = true })
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
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

    [Fact]
    internal Task IsNotEmptyAsync_ThrowWithNull()
    {
        return _testInstance
            .IsNotEmptyAsync((IAsyncEnumerable<int>)null, TestContext.Current.CancellationToken)
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
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await _testInstance
            .HasCountAsync(0, (IAsyncEnumerable<int>)null, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        await _testInstance
            .HasCountAsync(0, series, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        await _testInstance
            .HasCountAsync(1, series, canceler)
            .Assert()
            .ThrowsNoAsync<AssertException>(canceler);

        await _testInstance
            .HasCountAsync(2, series, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
    }

    [Theory, RandomData]
    internal async Task HasCountLessThanAsync_ThrowUnlessLess(
        [Size(1)] IAsyncEnumerable<object> series
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await _testInstance
            .HasCountLessThanAsync(0, (IAsyncEnumerable<int>)null, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        await _testInstance
            .HasCountLessThanAsync(0, series, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        await _testInstance
            .HasCountLessThanAsync(1, series, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        await _testInstance
            .HasCountLessThanAsync(2, series, canceler)
            .Assert()
            .ThrowsNoAsync<AssertException>(canceler);
    }

    [Theory, RandomData]
    internal async Task HasCountLessOrExactlyAsync_ThrowWhenGreater(
        [Size(1)] IAsyncEnumerable<object> series
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await _testInstance
            .HasCountLessOrExactlyAsync(0, (IAsyncEnumerable<int>)null, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        await _testInstance
            .HasCountLessOrExactlyAsync(0, series, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        await _testInstance
            .HasCountLessOrExactlyAsync(1, series, canceler)
            .Assert()
            .ThrowsNoAsync<AssertException>(canceler);

        await _testInstance
            .HasCountLessOrExactlyAsync(2, series, canceler)
            .Assert()
            .ThrowsNoAsync<AssertException>(canceler);
    }

    [Theory, RandomData]
    internal async Task HasCountMoreThanAsync_ThrowUnlessGreater(
        [Size(1)] IAsyncEnumerable<object> series
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await _testInstance
            .HasCountMoreThanAsync(0, (IAsyncEnumerable<int>)null, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        await _testInstance
            .HasCountMoreThanAsync(0, series, canceler)
            .Assert()
            .ThrowsNoAsync<AssertException>(canceler);

        await _testInstance
            .HasCountMoreThanAsync(1, series, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        await _testInstance
            .HasCountMoreThanAsync(2, series, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
    }

    [Theory, RandomData]
    internal async Task HasCountMoreOrExactlyAsync_ThrowUnlessGreater(
        [Size(1)] IAsyncEnumerable<object> series
    )
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        await _testInstance
            .HasCountMoreOrExactlyAsync(0, (IAsyncEnumerable<int>)null, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        await _testInstance
            .HasCountMoreOrExactlyAsync(0, series, canceler)
            .Assert()
            .ThrowsNoAsync<AssertException>(canceler);

        await _testInstance
            .HasCountMoreOrExactlyAsync(1, series, canceler)
            .Assert()
            .ThrowsNoAsync<AssertException>(canceler);

        await _testInstance
            .HasCountMoreOrExactlyAsync(2, series, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
    }

    [Theory, RandomData]
    internal async Task ContainsAsync_ThrowWithoutContent([Size(1)] IAsyncEnumerable<object> series)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        object item = (await AsyncSeriesHelper.ToListAsync(series, 2, canceler)).Single();

        await _testInstance
            .ContainsAsync(0, null, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        await _testInstance
            .ContainsAsync(item, series, canceler)
            .Assert()
            .ThrowsNoAsync<AssertException>(canceler);

        await _testInstance
            .ContainsAsync(await item.Tools().VariantAsync(canceler), series, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
    }

    [Theory, RandomData]
    internal async Task ContainsNotAsync_ThrowWithContent([Size(1)] IAsyncEnumerable<object> series)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        object item = (await AsyncSeriesHelper.ToListAsync(series, 2, canceler)).Single();

        await _testInstance
            .ContainsNotAsync(0, null, canceler)
            .Assert()
            .ThrowsNoAsync<AssertException>(canceler);

        await _testInstance
            .ContainsNotAsync(item, series, canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);

        await _testInstance
            .ContainsNotAsync(await item.Tools().VariantAsync(canceler), series, canceler)
            .Assert()
            .ThrowsNoAsync<AssertException>(canceler);
    }

    [Theory, RandomData]
    internal Task ThrowsAsync_ThrowWithNoError([Size(1)] IAsyncEnumerable<object> series)
    {
        return _testInstance
            .ThrowsExceptionAsync(series, TestContext.Current.CancellationToken)
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal async Task ThrowsAsync_MustMatchType(InvalidOperationException error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        async IAsyncEnumerable<int> throwError()
        {
            await Task.Delay(0, canceler);
            yield return 1;
            throw error;
        }

        await _testInstance
            .ThrowsAsync<InvalidOperationException, int>(throwError(), canceler)
            .Assert()
            .HasResultAsync(error, canceler);

        await _testInstance
            .ThrowsExceptionAsync(throwError(), canceler)
            .Assert()
            .HasResultAsync(error, canceler);

        await _testInstance
            .ThrowsAsync<ArgumentException, int>(throwError(), canceler)
            .Assert()
            .ThrowsAsync<AssertException>(canceler);
    }

    [Theory, RandomData]
    internal Task ThrowsAsync_UnwrapsError(ArgumentException error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        async IAsyncEnumerable<int> throwError()
        {
            await Task.Delay(0, canceler);
            yield return 1;
            throw new AggregateException(error);
        }

        return _testInstance
            .ThrowsAsync<ArgumentException, int>(throwError(), canceler)
            .Assert()
            .HasResultAsync(error, canceler);
    }

    [Theory, RandomData]
    internal async Task ThrowsAsync_AggregateWorks(Exception error1, Exception error2)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        AggregateException oneError = new(error1);
        AggregateException twoErrors = new(error1, error2);

        async IAsyncEnumerable<int> throwError(AggregateException error)
        {
            await Task.Delay(0, canceler);
            yield return 1;
            throw error;
        }

        await _testInstance
            .ThrowsAsync<AggregateException, int>(throwError(oneError), canceler)
            .Assert()
            .HasResultAsync(oneError, canceler);

        await _testInstance
            .ThrowsAsync<AggregateException, int>(throwError(twoErrors), canceler)
            .Assert()
            .HasResultAsync(twoErrors, canceler);
    }

    [Theory, RandomData]
    internal Task ThrowsAsync_RethrowsIfConfigured(ArgumentException error)
    {
        CancellationToken canceler = TestContext.Current.CancellationToken;

        async IAsyncEnumerable<int> throwError()
        {
            await Task.Delay(0, canceler);
            yield return 1;
            throw error;
        }

        return _testInstance
            .ThrowsAsync<ArgumentException, int>(
                throwError(),
                canceler,
                opt => opt with { DisableAssertThrowCatching = true }
            )
            .Assert()
            .ThrowsAsync<ArgumentException>(canceler)
            .That()
            .Is(error);
    }
}
