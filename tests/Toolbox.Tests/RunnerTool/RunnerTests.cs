using System.Reflection;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.Samples.ErrorCases;

namespace CreateAndFake.Tests.RunnerTool;

public static class RunnerTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            InjectionValues = [GetGeneratableMethod()],
            IgnorableExceptions = [typeof(ArgumentOutOfRangeException), typeof(ToolException)],
        };

    [Fact]
    internal static Task Runner_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<Runner>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task Runner_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<Runner>(
            TestContext.Current.CancellationToken,
            config
        );
    }

    private static MethodInfo GetGeneratableMethod()
    {
        return Tools.Randomizer.Create<MethodInfo>(opt =>
            opt with
            {
                FinalCondition = m =>
                    m is MethodInfo info
                    && !info.IsGenericMethod
                    && !info.IsGenericMethodDefinition,
            }
        );
    }

    [Theory, RandomData]
    internal static void CreateFor_InterfaceFakesInjected(
        Fake<IOnlyMockSample> fake,
        Fake<IOnlyMockSample> fake2
    )
    {
        Tools
            .Runner.CreateFor(typeof(InjectMockSample).GetConstructors().Single(), fake, fake2)
            .Args.ToArray()
            .Assert()
            .Is(new object[] { fake.Dummy, fake2.Dummy });
    }

    [Theory, RandomData]
    internal static void CreateFor_InjectedWorks(Injected<InjectMockSample> injected)
    {
        injected.Dummy.TestIfMockedSeparately();
    }

    [Theory, RandomData]
    internal static void CreateFor_InjectedNotManuallyInjected(
        Fake<IOnlyMockSample> inner1,
        Fake<IOnlyMockSample> inner2,
        InjectMockSample sample,
        Injected<InjectMockSample> injected
    )
    {
        injected.Dummy.Assert().IsNot(sample);
        injected.Fakes.Contains(inner1).Assert().Is(false);
        injected.Fakes.Contains(inner2).Assert().Is(false);

        injected.Dummy.TestIfMockedSeparately();
    }
}
