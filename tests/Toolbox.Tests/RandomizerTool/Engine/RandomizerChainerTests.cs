using CreateAndFake.RandomizerTool.Engine;
using CreateAndFake.Tests.TestSamples;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class RandomizerChainerTests
{
    [Fact]
    internal static Task RandomizerChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<IRandomizerChainer>();
    }

    [Fact]
    internal static Task RandomizerChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<IRandomizerChainer>();
    }

    [Fact]
    internal static void Create_HandlesInfinites()
    {
        Tools.Randomizer.Create<ChildWithParentSample>().Assert().IsNot(null);
        Tools.Randomizer.Create<ParentLoopSample>().Assert().IsNot(null);
    }
}
