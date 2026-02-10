using CreateAndFake.MutatorTool.Hints;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public static class LegacyCollectionMutateHintTests
{
    [Fact]
    internal static Task LegacyCollectionMutateHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<LegacyCollectionMutateHint>(
            TestContext.Current.CancellationToken
        );
    }
}
