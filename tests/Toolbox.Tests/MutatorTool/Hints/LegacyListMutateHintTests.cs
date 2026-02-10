using CreateAndFake.MutatorTool.Hints;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public static class LegacyListMutateHintTests
{
    [Fact]
    internal static Task LegacyListMutateHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<LegacyListMutateHint>(
            TestContext.Current.CancellationToken
        );
    }
}
