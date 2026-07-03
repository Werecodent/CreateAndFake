using System.Collections;
using System.Collections.Immutable;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.MutatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public sealed class ListMutateHintTests : MutateHintTestBase<ListMutateHint>
{
    [Fact]
    public void Modify_ImmutableSamplesFalse()
    {
        RunModifyTest<Array>(false, 0);
        RunModifyTest<int[]>(false, 0);
        RunModifyTest<ImmutableArray<int>>(false, 0);
        RunModifyTest<DataSample[]>(false, 0);
    }

    [Fact]
    public void Modify_ValueCollectionMutable()
    {
        RunModifyTest<Array>(true);
        RunModifyTest<int[]>(true);
        RunModifyTest<ArrayList>(true);
        RunModifyTest<ArrayList>(true, 0);
        RunModifyTest<DataSample[]>(true, 10);
    }

    [Theory, RandomData]
    public void Modify_UsesInternalType(DataSample value)
    {
        ArrayList data = new() { { value } };
        ArrayList original = data.Tools().Copy();

        Limiter.Score.StallUntil(
            "Until added to.",
            () => TestInstance.TryToModify(data, CreateChainer()),
            () => data.Count > original.Count,
            TestContext.Current.CancellationToken
        );

        data.Assert().IsNot(original);
        data.OfType<DataSample>().Assert().HasCount(2);
    }
}
