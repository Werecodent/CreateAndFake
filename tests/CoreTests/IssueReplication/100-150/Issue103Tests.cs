using CreateAndFake.Toolbox.AsserterTool;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue103Tests
{
    [Theory, RandomData]
    internal static void Issue103_AssertFailIncludesTestValue(string text)
    {
        string alt = Tools.Mutator.Variant(text);
        Tools.Asserter.Throws<AssertException>(() => text.Assert().Contains(alt)).Message.Assert().Contains(alt);
    }
}
