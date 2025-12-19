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
        for (int i = 0; i < 50; i++)
        {
            T sample = Tools.Randomizer.Create<T>();
            await Tools.AsyncAsserter.IsNotAsync(null, sample);
            await Tools.AsyncAsserter.IsNotAsync(sample, Tools.Mutator.Variant(sample));

            T dupe = Tools.Duplicator.Copy(sample);

            await Tools.AsyncAsserter.IsAsync(sample, dupe);
            await Tools.AsyncAsserter.IsAsync(
                await Tools.Valuer.GetHashCodeAsync(sample),
                await Tools.Valuer.GetHashCodeAsync(dupe)
            );
        }
    }
}
