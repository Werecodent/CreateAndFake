using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.IssueReplication;

public static class Issue103Tests
{
    [Theory, RandomData]
    internal static void Issue103_AssertFailIncludesTestValue(string text)
    {
        string alt = Tools.Mutator.Variant(text);
        Tools
            .Asserter.Throws<AssertException>(() => text.Assert().Contains(alt))
            .Message.Assert()
            .Contains(alt);
    }

    [Theory, RandomData]
    internal static void Issue103_AssertFailIncludesTestValueAndDetails(string text, string details)
    {
        string alt = Tools.Mutator.Variant(text);
        Tools
            .Asserter.Throws<AssertException>(() => text.Assert().Contains(alt, details))
            .Message.Assert()
            .Contains(alt)
            .And.Contains(details);
    }
}
