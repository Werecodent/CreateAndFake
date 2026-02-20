using CreateAndFake.RandomizerTool.Engine;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class RandomizerChainerTests
{
    [Fact]
    internal static Task RandomizerChainer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            new RandomizerChainer(Tools.Randomizer.Options, new RandomizerEngine()),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(ArgumentOutOfRangeException),
                        typeof(ArgumentException),
                    ],
                }
        );
    }

    [Fact]
    internal static Task RandomizerChainer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            new RandomizerChainer(Tools.Randomizer.Options, new RandomizerEngine()),
            TestContext.Current.CancellationToken,
            opt =>
                opt with
                {
                    IgnorableExceptions =
                    [
                        typeof(ArgumentOutOfRangeException),
                        typeof(ArgumentException),
                    ],
                }
        );
    }

    [Fact]
    internal static void Create_HandlesInfinites()
    {
        Tools.Randomizer.Create<ChildWithParentSample>().Assert().IsNot(null);
        Tools.Randomizer.Create<ParentLoopSample>().Assert().IsNot(null);
    }
}
