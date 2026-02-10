using CreateAndFake.MutatorTool.Hints;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public static class CollectionMutateHintTests
{
    [Fact]
    internal static Task CollectionMutateHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<CollectionMutateHint>(
            TestContext.Current.CancellationToken
        );
    }
}
