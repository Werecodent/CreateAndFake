using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.IssueReplication;

public static class Issue106Tests
{
    internal sealed class RandomNameContainer<T>
    {
        public T Content { get; set; }
    }

    public sealed class RandomNameItem
    {
        public string Message { get; set; }
    }

    [Theory, RandomData]
    internal static void Issue106_AssertIncludesGenericName(
        RandomNameContainer<RandomNameItem> generic
    )
    {
        generic
            .Assert(x => x.Assert().Is(generic.Tools().Variant()))
            .Throws<AssertException>()
            .With.Message.Assert()
            .Contains(nameof(RandomNameItem));
    }

    [Theory, RandomData]
    internal static void Issue106_AssertIncludesGenericNameCollections(
        List<Dictionary<string, RandomNameItem>> generic
    )
    {
        generic
            .Assert(x => x.Assert().Is(generic.Tools().Variant()))
            .Throws<AssertException>()
            .With.Message.Assert()
            .Contains(nameof(RandomNameItem));
    }
}
