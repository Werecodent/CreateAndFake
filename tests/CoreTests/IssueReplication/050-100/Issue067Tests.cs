namespace CreateAndFakeTests.IssueReplication;

public static class Issue067Tests
{
    [Fact]
    internal static void Issue067_SupportsUri()
    {
        TestSample<Uri>();
    }

    [Fact]
    internal static void Issue067_SupportsDateTimeOffset()
    {
        TestSample<DateTimeOffset>();
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
