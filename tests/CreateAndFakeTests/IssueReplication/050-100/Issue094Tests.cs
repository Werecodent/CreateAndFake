using CreateAndFake.Toolbox.DuplicatorTool;
using CreateAndFake.Toolbox.RandomizerTool;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue094Tests
{
    [Theory, RandomData]
    internal static void Issue094_StringSizeModifiedBySizeAttribute([Size(20)] string data)
    {
        data.Assert().HasCount(20);
    }

    [Theory, RandomData]
    internal static void Issue094_SizeAttributeOnlyTopCollection([Size(20)] IEnumerable<string> data)
    {
        data.Assert().HasCount(20);
        data.First().Length.Assert().IsNot(20);
    }

    [Theory, RandomData]
    internal static void Issue094_RandomizerOptionsWorks(RandomizerOptions options)
    {
        options.Assert().Is(options.CreateDeepClone());
    }

    [Theory, RandomData]
    internal static void Issue094_DuplicatorOptionsWorks(DuplicatorOptions options)
    {
        options.Assert().Is(options.CreateDeepClone());
    }
}