using System.Reflection;
using CreateAndFake.Design.Tooling;
using CreateAndFake.FakerTool;
using CreateAndFake.RandomizerTool;
using CreateAndFake.RandomizerTool.Engine;
using CreateAndFake.Samples.OldSamples;

namespace CreateAndFake.Tests.RandomizerTool;

public static class RandomizerTests
{
    [Fact]
    internal static Task Randomizer_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException<Randomizer>(opt =>
            opt with
            {
                InjectionValues = [GetGeneratableMethod()],
            }
        );
    }

    [Fact]
    internal static Task Randomizer_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation<Randomizer>(opt =>
            opt with
            {
                InjectionValues = [GetGeneratableMethod()],
            }
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
        new Randomizer(Tools.Randomizer.Options with { IncludeDefaultHints = false })
            .Assert(r => r.Create<object>())
            .Throws<ToolException>();
    }

    [Theory, RandomData]
    internal static void Create_MissingMatchThrows([Stub] CreateHint hint, string data)
    {
        hint.ToFake().ThrowByDefault = true;
        hint.ToFake()
            .Setup(
                m => m.TryCreate(data.GetType(), Arg.Any<IRandomizerChainer>()),
                Behavior.Returns(CreateHintResult.None, Times.Once)
            );

        new Randomizer(
            Tools.Randomizer.Options with
            {
                IncludeDefaultHints = false,
                Hints = [hint],
            }
        )
            .Assert(r => r.Create<string>())
            .Throws<ToolException>();

        hint.Assert().Called(Times.Once);
    }

    [Theory, RandomData]
    internal static void Create_ValidHintWorks([Stub] CreateHint hint, string data)
    {
        hint.ToFake().ThrowByDefault = true;
        hint.ToFake()
            .Setup(
                m => m.TryCreate(data.GetType(), Arg.Any<IRandomizerChainer>()),
                Behavior.Returns(new CreateHintResult(data), Times.Once)
            );

        new Randomizer(
            Tools.Randomizer.Options with
            {
                IncludeDefaultHints = false,
                Hints = [hint],
            }
        )
            .Create<string>()
            .Assert()
            .Is(data);

        hint.Assert().Called(Times.Once);
    }

    [Theory, RandomData]
    internal static void Create_InfiniteLoopDetails(Type type, [Fake] CreateHint hint)
    {
        hint.ToFake()
            .Setup(
                m => m.TryCreate(type, Arg.Any<IRandomizerChainer>()),
                Behavior.Throw<InsufficientExecutionStackException>(Times.Once)
            );

        new Randomizer(
            Tools.Randomizer.Options with
            {
                IncludeDefaultHints = false,
                Hints = [hint],
            }
        )
            .Assert(r => r.Create(type))
            .Throws<ToolException>()
            .Message.Assert()
            .Contains(type.Name);
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
            .Randomizer.Assert(r =>
                r.Create<DateTime>(opt =>
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
}
