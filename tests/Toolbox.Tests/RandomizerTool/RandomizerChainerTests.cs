using CreateAndFake.RandomizerTool;
using CreateAndFake.Tests.TestSamples;

namespace CreateAndFake.Tests.RandomizerTool;

public static class RandomizerChainerTests
{
    [Fact]
    internal static Task RandomizerChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<RandomizerChainer>();
    }

    [Fact]
    internal static Task RandomizerChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<RandomizerChainer>();
    }

    [Fact]
    internal static void Create_HandlesInfinites()
    {
        new RandomizerChainer(Tools.Randomizer.Options, (t, c) => c.Create<ParentLoopSample>())
            .Assert(c => c.Create(typeof(ChildWithParentSample), new ParentLoopSample()))
            .Throws<InfiniteLoopException>();
    }
}
