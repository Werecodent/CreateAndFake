using System.Collections.Immutable;
using Werecodent.CreateAndFake.MutatorTool.Hints;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Hints;

public sealed class ImmutableEnumerableMutateHintTests
    : MutateHintTestBase<ImmutableEnumerableMutateHint>
{
    [Fact]
    public void Modify_ImmutableSamplesFalse()
    {
        RunModifyTest<ImmutableQueue<int>>(false);
        RunModifyTest<ImmutableQueue<int>>(false, 0);
        RunModifyTest<ImmutableStack<DataSample>>(false, 0);
    }

    [Fact]
    public void Modify_MutableSamplesTrue()
    {
        RunModifyTest<ImmutableStack<DataSample>>(true, 10);
    }
}
