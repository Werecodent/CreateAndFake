using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.Tests.FakerTool.TestSamples;

namespace Werecodent.CreateAndFake.Tests.FakerTool;

public sealed class InjectedTests : AutoTester<Injected<InjectSample>>
{
    protected override Injected<InjectSample> TestInstance =>
        Tools.Randomizer.Create<Injected<InjectSample>>();

    protected override TesterMod OptionConfiguration =>
        opt =>
            opt with
            {
                InjectionValues = [TestInstance.Fakes],
                IgnorableExceptions = [typeof(InvalidOperationException)],
            };

    [Theory, RandomData]
    internal static void Fake_CanFindByDummy(Injected<FakeHolderSample> sample)
    {
        sample.Fake(sample.Dummy.Sample1).Assert().Is(sample.Fake<IFakeSample>());
        sample.Fake(sample.Dummy.Sample2).Assert().Is(sample.Fake<AbstractFakeSample>());
    }
}
