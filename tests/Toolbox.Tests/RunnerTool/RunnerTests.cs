using System.Reflection;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.Tests.TestSamples;

namespace CreateAndFake.Tests.RunnerTool;

public static class RunnerTests
{
    [Fact]
    internal static void Runner_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<Runner>(opt => opt with
        {
            InjectionValues = [GetGeneratableMethod()]
        });
    }

    [Fact]
    internal static void Runner_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation<Runner>(opt => opt with
        {
            InjectionValues = [GetGeneratableMethod()]
        });
    }

    private static MethodInfo GetGeneratableMethod()
    {
        return Tools.Randomizer.Create<MethodInfo>(opt => opt with
        {
            FinalCondition = m => m is MethodInfo info && !info.IsGenericMethod && !info.IsGenericMethodDefinition
        });
    }

    [Theory, RandomData]
    internal static void CreateFor_InterfaceFakesInjected(Fake<IOnlyMockSample> fake, Fake<IOnlyMockSample> fake2)
    {
        Tools.Runner
            .CreateFor(typeof(InjectMockSample).GetConstructors().Single(), fake, fake2).Args.ToArray()
            .Assert()
            .Is(new object[] { fake.Dummy, fake2.Dummy });
    }

    [Theory, RandomData]
    internal static void CreateFor_InjectedWorks(Injected<InjectMockSample> injected)
    {
        injected.Dummy.TestIfMockedSeparately();
    }

    [Theory, RandomData]
    internal static void CreateFor_InjectedNotManuallyInjected(Fake<IOnlyMockSample> inner1,
        Fake<IOnlyMockSample> inner2, InjectMockSample sample, Injected<InjectMockSample> injected)
    {
        injected.Dummy.Assert().IsNot(sample);
        injected.Fakes.Contains(inner1).Assert().Is(false);
        injected.Fakes.Contains(inner2).Assert().Is(false);

        injected.Dummy.TestIfMockedSeparately();
    }
}
