namespace CreateAndFake.Tests.IssueReplication;

public static class Issue016Tests
{
    internal sealed class Sample
    {
        public static int ValueA = 0;

        public int ValueB = 0;

        public static int ValueC { get; set; } = 0;
    }

    [Theory, RandomData]
    internal static void Issue016_StaticFieldsNotRandomized(Sample sample)
    {
        sample.ValueB.Assert().IsNot(0).Also(Sample.ValueA).Is(0);
    }

    [Theory, RandomData]
    internal static void Issue016_StaticPropertiesNotRandomized(Sample sample)
    {
        sample.ValueB.Assert().IsNot(0).Also(Sample.ValueC).Is(0);
    }
}
