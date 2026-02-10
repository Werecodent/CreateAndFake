using CreateAndFake.MutatorTool.Hints;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public static class LegacyDictionaryMutateHintTests
{
    [Fact]
    internal static Task LegacyDictionaryMutateHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<LegacyDictionaryMutateHint>(
            TestContext.Current.CancellationToken
        );
    }
}
