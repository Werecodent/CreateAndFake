using Werecodent.CreateAndFake.MutatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Hints;

public sealed class LegacyCollectionMutateHintTests : MutateHintTestBase<LegacyCollectionMutateHint>
{
    [Fact]
    public void Modify_DataCollectionMutable()
    {
        RunModifyTest<DataSample[]>(true, 10);
    }
}
