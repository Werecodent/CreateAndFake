using CreateAndFake.Toolbox.RandomizerTool;
using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.Toolbox.RandomizerTool;

public static class RandomizerChainerTests
{
    [Fact]
    internal static void RandomizerChainer_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<RandomizerChainer>();
    }

    [Fact]
    internal static void RandomizerChainer_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<RandomizerChainer>();
    }

    [Fact]
    internal static void Create_HandlesInfinites()
    {
        new RandomizerChainer(Tools.Randomizer.Options, (t, c) => c
            .Create<ParentLoopSample>())
            .Assert(c => c.Create(typeof(ChildWithParentSample), new ParentLoopSample()))
            .Throws<InfiniteLoopException>();
    }
}
