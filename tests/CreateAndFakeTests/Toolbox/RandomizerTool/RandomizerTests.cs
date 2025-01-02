using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.RandomizerTool;
using CreateAndFakeTests.TestSamples;

namespace CreateAndFakeTests.Toolbox.RandomizerTool;

public static class RandomizerTests
{
    private static readonly RandomizerOptions _ToolOptions = new()
    {
        Gen = Tools.Gen,
        Faker = Tools.Faker
    };

    [Fact]
    internal static void Randomizer_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException<Randomizer>();
    }

    [Fact]
    internal static void Create_NoRulesThrows()
    {
        new Randomizer(_ToolOptions with { IncludeDefaultHints = false })
            .Assert(r => r.Create<object>())
            .Throws<NotSupportedException>();
    }

    [Theory, RandomData]
    internal static void Create_MissingMatchThrows([Stub] CreateHint hint, string data)
    {
        hint.ToFake().ThrowByDefault = true;
        hint.ToFake().Setup(
            m => m.TryCreate(data.GetType(), Arg.Any<RandomizerChainer>()),
            Behavior.Returns((false, (object)null), Times.Once));

        new Randomizer(_ToolOptions with { IncludeDefaultHints = false, Hints = [hint] })
            .Assert(r => r.Create<string>())
            .Throws<NotSupportedException>();

        hint.Assert().Called(Times.Once);
    }

    [Theory, RandomData]
    internal static void Create_ValidHintWorks([Stub] CreateHint hint, string data)
    {
        hint.ToFake().ThrowByDefault = true;
        hint.ToFake().Setup(
            m => m.TryCreate(data.GetType(), Arg.Any<RandomizerChainer>()),
            Behavior.Returns((true, (object)data), Times.Once));

        new Randomizer(_ToolOptions with { IncludeDefaultHints = false, Hints = [hint] })
            .Create<string>()
            .Assert()
            .Is(data);

        hint.Assert().Called(Times.Once);
    }

    [Theory, RandomData]
    internal static void Create_InfiniteLoopDetails(Type type, [Fake] CreateHint hint)
    {
        hint.ToFake().Setup(
            m => m.TryCreate(type, Arg.Any<RandomizerChainer>()),
            Behavior.Throw<InsufficientExecutionStackException>(Times.Once));

        new Randomizer(
            Tools.Randomizer.Options with
            {
                IncludeDefaultHints = false,
                Hints = [hint]
            })
            .Assert(r => r.Create(type))
            .Throws<InsufficientExecutionStackException>().Message
            .Assert()
            .Contains(type.Name);
    }

    [Fact]
    internal static void Create_ConditionMatchReturned()
    {
        Tools.Randomizer
            .Create<int>(opt => opt with { FinalCondition = r => r is int v && v < 0 })
            .Assert()
            .LessThan(0);
    }

    [Fact]
    internal static void Create_ConditionTimesOut()
    {
        Tools.Randomizer
            .Assert(r => r.Create<DateTime>(opt => opt with
            {
                FinalCondition = r => r is DateTime d && d < DateTime.MinValue
            }))
            .Throws<TimeoutException>();
    }

    [Theory, RandomData]
    internal static void Inject_SingleFakeInjected(Fake<DataSample> fake, InjectSample holder)
    {
        holder.Data.Assert().ReferenceEqual(fake.Dummy);
        holder.Data2.Assert().ReferenceNotEqual(fake.Dummy);
    }

    [Theory, RandomData]
    internal static void Inject_DoubleFakeInjected(Fake<DataSample> fake, [Fake] DataSample fake2, InjectSample holder)
    {
        holder.Data2.Assert().ReferenceEqual(fake.Dummy);
        holder.Data.Assert().ReferenceEqual(fake2);
    }

    [Theory, RandomData]
    internal static void Inject_InterfaceFakesInjected(
        Fake<IOnlyMockSample> fake, Fake<IOnlyMockSample> fake2, InjectMockSample holder)
    {
        fake.Verify(Times.Never, f => f.FailIfNotMocked());
        fake2.Verify(Times.Never, f => f.FailIfNotMocked());
        holder.TestIfMockedSeparately();
        fake.Verify(Times.Once, f => f.FailIfNotMocked());
        fake2.Verify(Times.Once, f => f.FailIfNotMocked());
    }

    [Theory, RandomData]
    internal static void CreateFor_InterfaceFakesInjected(Fake<IOnlyMockSample> fake, Fake<IOnlyMockSample> fake2)
    {
        Tools.Randomizer
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
