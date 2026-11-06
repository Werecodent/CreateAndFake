using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.ValuerTool.Hints;

namespace Werecodent.CreateAndFake.Tests.ValuerTool.Hints;

public sealed class HandlerCompareHintTests : CompareHintTestBase<HandlerCompareHint>
{
    [Theory, RandomData]
    internal void Compare_CanIncludeCurrentSeed(SeededRandom gen, [Copy] SeededRandom gen2)
    {
        TestInstance
            .TryToCompare(
                gen,
                gen2,
                CreateChainer(Tools.Valuer.Options with { IgnoreCurrentRandomSeed = false })
            )
            .Data.Assert()
            .IsEmpty();

        _ = gen.Next<int>();

        TestInstance
            .TryToCompare(
                gen,
                gen2,
                CreateChainer(Tools.Valuer.Options with { IgnoreCurrentRandomSeed = false })
            )
            .Data.Assert()
            .IsNotEmpty();
    }
}
