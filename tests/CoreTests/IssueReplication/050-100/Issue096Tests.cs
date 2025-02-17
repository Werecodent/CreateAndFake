namespace CreateAndFakeTests.IssueReplication;

public static class Issue096Tests
{
    [Fact]
    internal static void Issue096_SupportsIAsyncEnumerable()
    {
        TestSample<IAsyncEnumerable<int>>();
        TestSample<IAsyncEnumerable<string>>();
        TestSample<IAsyncEnumerable<object>>();
    }

    [Theory, RandomData]
    internal static async Task Issue096_SupportsSizedAsyncEnumerable([Size(5)] IAsyncEnumerable<int> items)
    {
        int count = 0;
        await foreach (int item in items)
        {
            count++;
        }
        count.Assert().Is(5);
    }

    private static void TestSample<T>()
    {
        for (int i = 0; i < 50; i++)
        {
            T sample = Tools.Randomizer.Create<T>();
            Tools.Asserter.IsNot(null, sample);
            Tools.Asserter.IsNot(sample, Tools.Mutator.Variant(sample));

            T dupe = Tools.Duplicator.Copy(sample);

            Tools.Asserter.Is(sample, dupe);
            Tools.Asserter.Is(Tools.Valuer.GetHashCode(sample), Tools.Valuer.GetHashCode(dupe));
        }
    }
}
