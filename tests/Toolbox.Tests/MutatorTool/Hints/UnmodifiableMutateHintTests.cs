using CreateAndFake.MutatorTool.Engine;
using CreateAndFake.MutatorTool.Hints;

namespace CreateAndFake.Tests.MutatorTool.Hints;

public sealed class UnmodifiableMutateHintTests : MutateHintTestBase<UnmodifiableMutateHint>
{
    [Theory, RandomData]
    internal void TryToModify_EnumIncluded(Enum flags)
    {
        TestInstance.TryToModify(flags, CreateChainer()).Assert().Is(new MutateHintResult(false));
    }
}
