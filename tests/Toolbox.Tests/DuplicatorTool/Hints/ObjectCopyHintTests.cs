using Werecodent.CreateAndFake.DuplicatorTool.Engine;
using Werecodent.CreateAndFake.DuplicatorTool.Hints;
using Werecodent.CreateAndFake.Samples.ErrorCases;
using Werecodent.CreateAndFake.Samples.Scenarios;

namespace Werecodent.CreateAndFake.Tests.DuplicatorTool.Hints;

public sealed class ObjectCopyHintTests : CopyHintTestBase<ObjectCopyHint>
{
    private static readonly Type[] _ValidTypes =
    [
        typeof(PrivateValuerEquatableSample),
        typeof(IUnimplementedSample),
        typeof(DataHolderSample),
        typeof(FieldSample),
        typeof(DataHolderSample),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(MismatchDataSample),
        typeof(FactoryAlterSample),
    ];

    public ObjectCopyHintTests()
        : base(_ValidTypes, _InvalidTypes) { }

    public override Task Tester_NoParameterMutation()
    {
        return Task.CompletedTask;
    }

    [Fact]
    internal void Copy_IgnoresBadSetters()
    {
        IsBadSample sample = new();
        TestInstance.TryCopy(sample, CreateChainer()).Data.Assert().Is(sample);
    }

    [Theory, RandomData]
    internal void Copy_DuplicateAware(DataSample sample, DataSample fakeClone)
    {
        IDuplicatorChainer chainer = CreateChainer();
        chainer.AddToHistory(sample, fakeClone);

        TestInstance.TryCopy(sample, chainer).Data.Assert().Is(fakeClone);
    }
}
