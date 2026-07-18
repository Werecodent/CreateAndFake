using System.Reflection;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.Design.Types;
using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RandomizerTool.Engine;
using CreateAndFake.Samples.ErrorCases;
using CreateAndFake.Samples.Scenarios;

namespace CreateAndFake.Tests.RandomizerTool;

public static class RandomizerTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnorableExceptions = [typeof(ArgumentException), typeof(ArgumentOutOfRangeException)],
            InjectionValues = [GetGeneratableMethod()],
            MethodsToIgnore = [nameof(Randomizer.Inject)],
        };

    [Fact]
    internal static Task Randomizer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<Randomizer>(
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task Randomizer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync<Randomizer>(
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

    [Fact]
    internal static void Create_NoRulesThrows()
    {
        new Randomizer(Tools.Randomizer.Options with { IncludeFrameworkHints = false })
            .Assert(x => x.Create<object>())
            .Throws<ToolException>();
    }

    [Theory, RandomData]
    internal static void Create_MissingMatchThrows([Stub] CreateHint hint, string data)
    {
        hint.Tools().ToFake().ThrowByDefault = true;
        hint.Tools()
            .ToFake()
            .Setup(
                m => m.TryToCreate(data.GetType(), Arg.Any<IRandomizerChainer>()),
                Behavior.Returns(CreateHintResult.None, Times.Once)
            );

        new Randomizer(
            Tools.Randomizer.Options with
            {
                IncludeFrameworkHints = false,
                Hints = [hint],
            }
        )
            .Assert(x => x.Create<string>())
            .Throws<ToolException>();

        hint.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Create_ValidHintWorks([Stub] CreateHint hint, string data)
    {
        hint.Tools().ToFake().ThrowByDefault = true;
        hint.Tools()
            .ToFake()
            .Setup(
                m => m.TryToCreate(data.GetType(), Arg.Any<IRandomizerChainer>()),
                Behavior.Returns(new CreateHintResult(data), Times.Once)
            );

        new Randomizer(
            Tools.Randomizer.Options with
            {
                IncludeFrameworkHints = false,
                Hints = [hint],
            }
        )
            .Create<string>()
            .Assert()
            .Is(data);

        hint.Assert().Called();
    }

    [Theory, RandomData]
    internal static void Create_InfiniteLoopDetails(Type type, [Fake] CreateHint hint)
    {
        hint.Tools()
            .ToFake()
            .Setup(
                m => m.TryToCreate(type, Arg.Any<IRandomizerChainer>()),
                Behavior.Throw<InsufficientExecutionStackException>(Times.Once)
            );

        new Randomizer(
            Tools.Randomizer.Options with
            {
                IncludeFrameworkHints = false,
                Hints = [hint],
            }
        )
            .Assert(x => x.Create(type))
            .Throws<ToolException>()
            .With.Message.Assert()
            .Contains(GenericConverter.ExpandName(type));
    }

    [Fact]
    internal static void Create_ConditionMatchReturned()
    {
        Tools
            .Randomizer.Create<int>(opt => opt with { FinalCondition = r => r is int v && v < 0 })
            .Assert()
            .LessThan(0);
    }

    [Fact]
    internal static void Create_ConditionTimesOut()
    {
        Tools
            .Randomizer.Assert(x =>
                x.Create<DateTime>(opt =>
                    opt with
                    {
                        FinalCondition = r => r is DateTime d && d < DateTime.MinValue,
                    }
                )
            )
            .Throws<ToolException>();
    }

    [Theory, RandomData]
    internal static void Inject_SingleFakeInjected(Fake<DataSample> fake, InjectSample holder)
    {
        holder.Data.Assert().ReferenceEqual(fake.Dummy);
        holder.Data2.Assert().ReferenceNotEqual(fake.Dummy);
    }

    [Theory, RandomData]
    internal static void Inject_DoubleFakeInjected(
        Fake<DataSample> fake,
        [Fake] DataSample fake2,
        InjectSample holder
    )
    {
        holder.Data2.Assert().ReferenceEqual(fake.Dummy);
        holder.Data.Assert().ReferenceEqual(fake2);
    }

    [Theory, RandomData]
    internal static void Inject_InterfaceFakesInjected(
        Fake<IOnlyMockSample> fake,
        Fake<IOnlyMockSample> fake2,
        InjectMockSample holder
    )
    {
        fake.Verify(Times.Never, f => f.FailIfNotMocked());
        fake2.Verify(Times.Never, f => f.FailIfNotMocked());
        holder.TestIfMockedSeparately();
        fake.Verify(Times.Once, f => f.FailIfNotMocked());
        fake2.Verify(Times.Once, f => f.FailIfNotMocked());
    }

    [Fact]
    internal static void Create_HandlesInfinites()
    {
        Tools.Randomizer.Create<ChildWithParentSample>().Assert().IsNotNull();
        Tools.Randomizer.Create<ParentLoopSample>().Assert().IsNotNull();
    }
}
