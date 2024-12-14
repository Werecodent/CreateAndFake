using CreateAndFake.Toolbox.FakerTool;
using CreateAndFakeTests.TestSamples;
using CreateAndFakeTests.Toolbox.FakerTool.TestSamples;

namespace CreateAndFakeTests.Toolbox.FakerTool;

public static class InjectedTests
{
    [Theory, RandomData]
    internal static void Injected_GuardsNulls(Injected<InjectSample> sample)
    {
        Tools.Tester.PreventsNullRefException(sample, opt => opt with { InjectionValues = [sample.Fakes] });
    }

    [Theory, RandomData]
    internal static void Injected_NoParameterMutation(Injected<InjectSample> sample)
    {
        Tools.Tester.PreventsParameterMutation(sample, opt => opt with { InjectionValues = [sample.Fakes] });
    }

    [Theory, RandomData]
    internal static void Fake_CanFindByDummy(Injected<FakeHolderSample> sample)
    {
        sample.Fake(sample.Dummy.Sample1).Assert().Is(sample.Fake<IFakeSample>());
        sample.Fake(sample.Dummy.Sample2).Assert().Is(sample.Fake<AbstractFakeSample>());
    }
}
