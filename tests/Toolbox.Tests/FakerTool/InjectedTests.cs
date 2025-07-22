using CreateAndFake.FakerTool;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.Tests.FakerTool.TestSamples;

namespace CreateAndFake.Tests.FakerTool;

public static class InjectedTests
{
    [Theory, RandomData]
    internal static Task Injected_GuardsNulls(Injected<InjectSample> sample)
    {
        return Tools.Tester.PreventsNullRefException(
            sample,
            opt => opt with { InjectionValues = [sample.Fakes] }
        );
    }

    [Theory, RandomData]
    internal static Task Injected_NoParameterMutation(Injected<InjectSample> sample)
    {
        return Tools.Tester.PreventsParameterMutation(
            sample,
            opt => opt with { InjectionValues = [sample.Fakes] }
        );
    }

    [Theory, RandomData]
    internal static void Fake_CanFindByDummy(Injected<FakeHolderSample> sample)
    {
        sample.Fake(sample.Dummy.Sample1).Assert().Is(sample.Fake<IFakeSample>());
        sample.Fake(sample.Dummy.Sample2).Assert().Is(sample.Fake<AbstractFakeSample>());
    }
}
