using CreateAndFake.AsserterTool;

namespace CreateAndFake.Tests.IssueReplication;

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
        Tools
            .Asserter.Assert(a => a.Fail())
            .Throws<AssertException>()
            .Exception.Message.Assert()
            .Contains($"{Tools.Gen.InitialSeed}");
    }
}
