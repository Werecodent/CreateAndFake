using Werecodent.CreateAndFake.MutatorTool.Engine;
using Werecodent.CreateAndFake.MutatorTool.Hints;

namespace Werecodent.CreateAndFake.Tests.MutatorTool.Hints;

public sealed class UnmodifiableMutateHintTests : MutateHintTestBase<UnmodifiableMutateHint>
{
    [Theory, RandomData]
    internal void TryToModify_EnumIncluded(Enum flags)
    {
        TestInstance.TryToModify(flags, CreateChainer()).Assert().Is(new MutateHintResult(false));
    }
}
