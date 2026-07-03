using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.MutatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public sealed class CollectionMutateHintTests : MutateHintTestBase<CollectionMutateHint>
{
    [Fact]
    public void Alter_ImmutableSamplesFalse()
    {
        RunModifyTest<int[]>(false);
        RunModifyTest<int[]>(false, 0);
        RunModifyTest<DataSample[]>(false, 0);
    }

    [Fact]
    public void Alter_MutableSamplesTrue()
    {
        RunModifyTest<DataSample[]>(true, 10);
        RunModifyTest<List<int>>(true);
        RunModifyTest<List<int>>(true, 0);
    }

    [Theory, RandomData]
    public void Alter_AddsToMutableCollection(List<DataSample> data)
    {
        List<DataSample> original = data.Tools().Copy();

        TestInstance.TryToModify(data, CreateChainer()).Assert().Is(new MutateHintResult(true));

        data.Assert().HasCount(original.Count + 1);
    }
}
