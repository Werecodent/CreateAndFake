using CreateAndFake.Toolbox.AsserterTool;

namespace CreateAndFakeTests.IssueReplication;

public static class Issue081Tests
{
    [Fact]
    internal static void Issue081_RandomizationIsSeeded()
    {
        Tools.Gen.InitialSeed.Assert().IsNot(null);
    }

    [Fact]
    internal static void Issue081_AssertionsContainSeed()
    {
        Tools.Asserter
            .Assert(a => a.Fail())
            .Throws<AssertException>().Message
            .Assert()
            .Contains($"{Tools.Gen.InitialSeed}");
    }
}
