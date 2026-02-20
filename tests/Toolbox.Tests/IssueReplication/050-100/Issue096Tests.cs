namespace CreateAndFake.Tests.IssueReplication;

public static class Issue096Tests
{
    [Fact]
    internal static async Task Issue096_SupportsIAsyncEnumerable()
    {
        await TestSample<IAsyncEnumerable<int>>();
        await TestSample<IAsyncEnumerable<string>>();
        await TestSample<IAsyncEnumerable<object>>();
    }

    [Theory, RandomData]
    internal static async Task Issue096_SupportsSizedAsyncEnumerable(
        [Size(5)] IAsyncEnumerable<int> items
    )
    {
        int count = 0;
        await foreach (int item in items)
        {
            count++;
        }
        count.Assert().Is(5);
    }

    private static async Task TestSample<T>()
    {
        var ct = TestContext.Current.CancellationToken;
        for (int i = 0; i < 50; i++)
        {
            T sample = Tools.Randomizer.Create<T>();
            await Tools.Asserter.IsNotAsync(null, sample, ct);
            await Tools.Asserter.IsNotAsync(sample, Tools.Mutator.Variant(sample), ct);

            T dupe = Tools.Duplicator.Copy(sample);

            await Tools.Asserter.IsAsync(sample, dupe, ct);
            await Tools.Asserter.IsAsync(
                await Tools.Valuer.GetHashCodeAsync(sample, ct),
                await Tools.Valuer.GetHashCodeAsync(dupe, ct),
                ct
            );
        }
    }
}
