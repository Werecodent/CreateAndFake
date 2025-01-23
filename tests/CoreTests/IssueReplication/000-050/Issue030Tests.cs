namespace CreateAndFakeTests.IssueReplication;

public static class Issue030Tests
{
    [Fact]
    internal static void Issue030_SupportsGuid()
    {
        TestSample<Guid>();
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
