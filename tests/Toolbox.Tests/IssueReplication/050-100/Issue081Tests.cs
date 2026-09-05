using Werecodent.CreateAndFake.AsserterTool;

namespace Werecodent.CreateAndFake.Tests.IssueReplication;

public static class Issue081Tests
{
    [Fact]
    internal static void Issue081_RandomizationIsSeeded()
    {
        Tools.Gen.InitialSeed.Assert().IsNotNull();
    }

    [Fact]
    internal static void Issue081_AssertionsContainSeed()
    {
        Tools
            .Asserter.Assert(x => x.Fail())
            .Throws<AssertException>()
            .With.Message.Assert()
            .Contains($"{Tools.Gen.InitialSeed}");
    }
}
