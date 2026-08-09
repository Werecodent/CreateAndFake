using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.RandomizerTool.Hints;
using Werecodent.CreateAndFake.Samples.ErrorCases;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class ObjectCreateHintTests : CreateHintTestBase<ObjectCreateHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(DataHolderSample),
        typeof(Arg),
        typeof(DataHolderSample),
        typeof(IUnimplementedSample),
        typeof(FieldSample),
        typeof(FactoryAlterSample),
    ];

    private static readonly Type[] _InvalidTypes = [typeof(VoidType)];

    public ObjectCreateHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

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
