using CreateAndFake.Design.Exceptions;
using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool.Hints;
using CreateAndFake.Samples.ErrorCases;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.Tests.FakerTool.TestSamples;

namespace CreateAndFake.Tests.RandomizerTool.Hints;

public sealed class InjectedCreateHintTests : CreateHintTestBase<InjectedCreateHint>
{
    private static readonly InjectedCreateHint _TestInstance = new();

    private static readonly Type[] _ValidTypes =
    [
        typeof(Injected<FakeHolderSample>),
        typeof(Injected<InjectSample>),
        typeof(Injected<InjectMockSample>),
        typeof(Injected<MismatchDataSample>),
        typeof(Injected<StructSample>),
    ];

    private static readonly Type[] _InvalidTypes =
    [
        typeof(DataHolderSample),
        typeof(IUnimplementedSample),
    ];

    public InjectedCreateHintTests()
        : base(_TestInstance, _ValidTypes, _InvalidTypes) { }

    [Theory, RandomData]
    internal void Create_ValidInjections(Injected<InjectMockSample> sample)
    {
        sample.Fake<IOnlyMockSample>().Assert().IsNotNull();
        sample
            .Fake<IOnlyMockSample>(1)
            .Assert()
            .IsNotNull()
            .And.IsNot(sample.Fake<IOnlyMockSample>());

        sample
            .Fake<IOnlyMockSample>()
            .Setup(m => m.FailIfNotMocked(), Behavior.Returns(false, Times.Once));
        sample.Dummy.TestIfMockedSeparately();
        sample.Fake<IOnlyMockSample>().Verify(Times.Once);
        sample.Verify();
    }

    [Fact]
    internal void Create_NoConstructorThrows()
    {
        Tools
            .Randomizer.Assert(x => x.Create<Injected<IUnimplementedSample>>())
            .Throws<ToolException>();
    }

    [Theory, RandomData]
    internal void Create_ValuesRandom(Injected<FakeHolderSample> sample)
    {
        sample.Dummy.Value1.Assert().IsNot(0);
        sample.Dummy.Value2.Assert().IsNotNull();
    }
}
