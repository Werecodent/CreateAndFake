using CreateAndFake.MutatorTool.Hints;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public static class ImmutableEnumerableMutateHintTests
{
    [Fact]
    internal static Task ImmutableEnumerableMutateHint_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<ImmutableEnumerableMutateHint>(
            TestContext.Current.CancellationToken
        );
    }
}
