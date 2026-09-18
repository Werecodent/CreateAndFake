using Werecodent.CreateAndFake.MutatorTool.Engine;
using Werecodent.CreateAndFake.MutatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Hints;

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

        TestInstance
            .TryToModify(data, CreateChainer())
            .Assert()
            .Is(new MutateHintResult(true))
            .Also(data)
            .HasCount(original.Count + 1);
    }
}
