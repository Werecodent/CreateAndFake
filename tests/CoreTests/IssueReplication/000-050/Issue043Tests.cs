namespace CreateAndFakeTests.IssueReplication;

public static class Issue043Tests
{
    [Fact]
    internal static void Issue043_SupportsTask()
    {
        TestSample<Task>();
    }

    [Fact]
    internal static void Issue043_SupportsGenericTask()
    {
        TestSample<Task<string>>();
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
