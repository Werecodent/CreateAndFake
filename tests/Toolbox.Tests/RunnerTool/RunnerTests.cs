using System.Reflection;
using Werecodent.CreateAndFake.Design.Exceptions;
using Werecodent.CreateAndFake.Design.Types;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.Samples.ErrorCases;

namespace Werecodent.CreateAndFake.Tests.RunnerTool;

public static class RunnerTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            InjectionValues = [GetGeneratableMethod()],
            IgnorableExceptions =
            [
                typeof(ArgumentOutOfRangeException),
                typeof(ToolException),
                typeof(UnsupportedException),
            ],
        };

    [Fact]
    internal static Task Runner_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<Runner>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task Runner_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<Runner>(
            TestContext.Current.CancellationToken,
            _Config
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
            .Runner.CreateFor(
                TypeDescriber.For<InjectMockSample>().Constructors.OnlyPublic.Single(),
                opt => opt with { InjectionValues = [fake, fake2] },
                TestContext.Current.CancellationToken
            )
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
