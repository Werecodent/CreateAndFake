using CreateAndFake.MutatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public sealed class LegacyCollectionMutateHintTests : MutateHintTestBase<LegacyCollectionMutateHint>
{
    [Fact]
    public void Modify_DataCollectionMutable()
    {
        RunModifyTest<DataSample[]>(true, 10);
    }
}
