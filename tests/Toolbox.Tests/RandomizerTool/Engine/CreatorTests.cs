using CreateAndFake.RandomizerTool.Engine;

namespace CreateAndFake.Tests.RandomizerTool.Engine;

public static class CreatorTests
{
    [Fact]
    internal static Task Creator_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<Creator>(
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static Task Creator_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<Creator>(
            TestContext.Current.CancellationToken
        );
    }
}
