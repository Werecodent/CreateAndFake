using CreateAndFake.Design.Reiteration;
using CreateAndFake.Design.Tooling;
using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.Samples.ErrorCases;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class ObjectCreateHintTests : CreateHintTestBase<ObjectCreateHint>
{
    private static readonly ObjectCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(object),
        typeof(Arg),
        typeof(DataHolderSample),
        typeof(IUnimplementedSample),
        typeof(FieldSample),
        typeof(FactorySample),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(VoidType)];

    public ObjectCreateHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    public void ObjectCreateHint_CanHandleInfinites(
        InfiniteSample sample1,
        ParentLoopSample sample2
    )
    {
        sample1.Assert().IsNot(null);
        sample2.Assert().IsNot(null);
    }

    [Fact]
    public void Create_FailsWithBadSample()
    {
        typeof(IIsGoodOrBadSample)
            .Assert(type =>
                Limiter.Hundred.Repeat(
                    "Retries until failing to pick the good sample.",
                    () =>
                        type.CreateRandomInstance(opt =>
                            opt with
                            {
                                ObjectCreateAttempts = Limiter.Once,
                            }
                        )
                )
            )
            .Throws<ToolException>();
    }

    [Fact]
    public void Create_RetriesUntilGoodSample()
    {
        typeof(IIsGoodOrBadSample)
            .CreateRandomInstance(opt => opt with { ObjectCreateAttempts = Limiter.Hundred })
            .GetType()
            .Assert()
            .Is(typeof(IsGoodSample));
    }
}
