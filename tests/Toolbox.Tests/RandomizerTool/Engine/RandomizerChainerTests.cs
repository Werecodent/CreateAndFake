using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class RandomizerChainerTests
{
    [Fact]
    internal static void Create_HandlesInfinites()
    {
        Tools.Randomizer.Create<ChildWithParentSample>().Assert().IsNot(null);
        Tools.Randomizer.Create<ParentLoopSample>().Assert().IsNot(null);
    }
}
