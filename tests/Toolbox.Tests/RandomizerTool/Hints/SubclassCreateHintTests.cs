using Werecodent.CreateAndFake.RandomizerTool.Engine;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.ErrorCases;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class SubclassCreateHintTests : CreateHintTestBase<SubclassCreateHint>
{
    private static readonly Type[] _ValidTypes = [typeof(IIsGoodOrBadSample)];

    private static readonly Type[] _InvalidTypes = [typeof(VoidType)];

    public SubclassCreateHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    [Fact]
    internal void TryToCreate_CanNotPreferLocalSubclasses()
    {
        TestInstance
            .TryToCreate(
                typeof(IIsGoodOrBadSample),
                CreateChainer(Tools.Randomizer.Options with { PreferLocalSubclasses = false })
            )
            .HasData.Assert()
            .Is(true);
    }

    [Theory, RandomData]
    internal void TryToCreate_ContinuesAfterExceptions(
        [Stub] IRandomizerChainer randomizer,
        IsGoodSample sample
    )
    {
        randomizer.AlreadyCreated(Arg.Any<Type>()).SetupReturn(false);
        randomizer.Options.SetupReturn(Tools.Randomizer.Options);
        randomizer
            .CreateSpecific(
                typeof(IsBadSample),
                typeof(IIsGoodOrBadSample),
                Arg.Any<RandomizerMod>()
            )
            .SetupReturn(Behavior<object>.Throw<InvalidOperationException>());
        randomizer
            .CreateSpecific(
                typeof(IsGoodSample),
                typeof(IIsGoodOrBadSample),
                Arg.Any<RandomizerMod>()
            )
            .SetupReturn(sample);

        for (int i = 0; i < 10; i++)
        {
            TestInstance
                .TryToCreate(typeof(IIsGoodOrBadSample), randomizer)
                .Assert()
                .Is(new CreateHintResult(sample));
        }
    }
}
