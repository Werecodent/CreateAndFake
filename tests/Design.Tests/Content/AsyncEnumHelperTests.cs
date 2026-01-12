using CreateAndFake.Design.Content;

namespace CreateAndFake.Design.Tests.Content;

public static class AsyncEnumHelperTests
{
    [Fact]
    internal static Task AsyncEnumHelper_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            typeof(AsyncEnumHelper),
            TestContext.Current.CancellationToken
        );
    }

    /*[Fact]
    internal static Task AsyncEnumHelper_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            typeof(AsyncEnumHelper),
            TestContext.Current.CancellationToken
        );
    }*/

    [Theory, RandomData]
    internal static async Task CreateFrom_ConvertsObjectsSuccessfully(IList<string> data)
    {
        int i = 0;
        await foreach (
            string value in AsyncEnumHelper
                .CreateFrom(data)
                .WithCancellation(TestContext.Current.CancellationToken)
        )
        {
            value.Assert().Is(data[i++]);
        }
        i.Assert().Is(data.Count);
    }

    [Theory, RandomData]
    internal static async Task CreateFrom_CanBeCanceled(IList<string> data)
    {
        try
        {
            await foreach (
                string value in AsyncEnumHelper
                    .CreateFrom(data)
                    .WithCancellation(new CancellationToken(true))
            )
            {
                value.Assert().Fail();
            }
        }
        catch (OperationCanceledException)
        {
            data.Assert().Pass();
        }
    }

    [Fact]
    internal static async Task HasAnyAsync_FalseWhenGivenNull()
    {
        (await AsyncEnumHelper.HasAnyAsync<string>(null, TestContext.Current.CancellationToken))
            .Assert()
            .Is(false);
    }

    [Fact]
    internal static async Task HasAnyAsync_FalseWithNoValues()
    {
        IAsyncEnumerable<string> data = AsyncEnumHelper.CreateFrom<string>([]);

        (await AsyncEnumHelper.HasAnyAsync(data, TestContext.Current.CancellationToken))
            .Assert()
            .Is(false);
    }

    [Theory, RandomData]
    internal static async Task HasAnyAsync_TrueWithValues(IAsyncEnumerable<string> data)
    {
        (await AsyncEnumHelper.HasAnyAsync(data, TestContext.Current.CancellationToken))
            .Assert()
            .Is(true);
    }

    [Fact]
    internal static Task HasAnyAsync_CanBeCanceledInitially()
    {
        IAsyncEnumerable<string> data = AsyncEnumHelper.CreateFrom<string>([]);

        return AsyncEnumHelper
            .HasAnyAsync(data, new CancellationToken(true))
            .Assert(async t => await t)
            .Throws<OperationCanceledException>();
    }

    [Theory, RandomData]
    internal static async Task ToListAsync_ConvertsValues(IAsyncEnumerable<string> data)
    {
        IList<string> results = await AsyncEnumHelper.ToListAsync(
            data,
            TestContext.Current.CancellationToken
        );

        int i = 0;
        await foreach (string value in data.WithCancellation(TestContext.Current.CancellationToken))
        {
            value.Assert().Is(results[i++]);
        }
        i.Assert().Is(results.Count);
    }

    /*[Theory, RandomData]
    internal static async Task ToListAsync_CanBeCanceledMidway(IList<string> data)
    {
        using CancellationTokenSource source = new();

        await AsyncEnumHelper
            .ToListAsync(AsyncEnumHelper.CreateCancelingIteration(data, source), source.Token)
            .Assert(async t => await t)
            .Throws<OperationCanceledException>();
    }*/
}
