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
        typeof(DataHolderSample),
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
        sample1.Assert().IsNotNull();
        sample2.Assert().IsNotNull();
    }
}
