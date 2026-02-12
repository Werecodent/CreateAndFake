using System.Collections;
using CreateAndFake.MutatorTool.Hints;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public sealed class ObjectMutateHintTests : MutateHintTestBase<ObjectMutateHint>
{
    public ObjectMutateHintTests()
        : base([typeof(DataHolderSample)], [typeof(ICollection)]) { }
}
