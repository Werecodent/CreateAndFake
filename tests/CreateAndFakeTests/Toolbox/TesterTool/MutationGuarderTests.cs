using CreateAndFake.Toolbox.AsserterTool;
using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.TesterTool;
using CreateAndFakeTests.TestSamples;
using CreateAndFakeTests.Toolbox.TesterTool.TestSamples;

namespace CreateAndFakeTests.Toolbox.TesterTool;

public static class MutationGuarderTests
{
    private static readonly MutationGuarder _ShortTestInstance = new(
        new GenericFixer(Tools.Tester.Options), Tools.Randomizer,
        Tools.Duplicator, Tools.Asserter, new TimeSpan(0, 0, 0, 0, 100));

    private static readonly MutationGuarder _LongTestInstance = new(
        new GenericFixer(Tools.Tester.Options), Tools.Randomizer,
        Tools.Duplicator, Tools.Asserter, new TimeSpan(0, 0, 10));

    [Fact]
    internal static void MutationGuarder_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(_ShortTestInstance);
    }

    [Fact]
    internal static void MutationGuarder_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation(_ShortTestInstance);
    }

    [Fact]
    internal static void PreventsParameterMutation_OnStatics()
    {
        Tools.Tester
            .Assert(t => t.PreventsParameterMutation(typeof(StaticMutationSample)))
            .Throws<AssertException>();
    }

    [Fact]
    internal static void PreventsParameterMutation_StatelessFine()
    {
        Tools.Tester.PreventsParameterMutation<StatelessSample>();
    }

    [Fact]
    internal static void PreventsParameterMutation_NullInjectionsFine()
    {
        Tools.Tester.PreventsParameterMutation<StatelessSample>((object[])null);
    }

    [Theory, RandomData]
    internal static void PreventsParameterMutation_InjectsMultipleValues(Fake<IOnlyMockSample> fake1, Fake<IOnlyMockSample> fake2)
    {
        Tools.Tester.PreventsParameterMutation<InjectMockSample>(fake1, fake2);
    }

    [Theory, RandomData]
    internal static void PreventsParameterMutation_InjectsWithMethods(Fake<IOnlyMockSample> fake)
    {
        Tools.Tester.PreventsParameterMutation<MockMethodPassOnly>(fake);
    }

    [Fact]
    internal static void CallMethod_TimesOut()
    {
        _ShortTestInstance
            .Assert(t => t.PreventsMutationOnStatics(typeof(LongMethodSample), false))
            .Throws<TimeoutException>();
    }

    [Fact]
    internal static void PreventsMutationOnConstructors_Disposes()
    {
        lock (MockDisposableSample._Lock)
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = Tools.Faker.Stub<IDisposable>();

            _LongTestInstance.PreventsMutationOnConstructors(typeof(MockDisposableSample), true);
            Tools.Asserter.Is(1, MockDisposableSample._ClassDisposes);
            Tools.Asserter.Is(0, MockDisposableSample._FinalizerDisposes);
            MockDisposableSample._Fake.Verify(Times.Once, d => d.Dispose());
        }
    }

    [Fact]
    internal static void PreventsMutationOnMethods_Disposes()
    {
        lock (MockDisposableSample._Lock)
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = Tools.Faker.Stub<IDisposable>();

            using MockDisposableSample sample = new(null);
            _LongTestInstance.PreventsMutationOnMethods(sample);
            Tools.Asserter.Is(0, MockDisposableSample._ClassDisposes);
            Tools.Asserter.Is(0, MockDisposableSample._FinalizerDisposes);
            MockDisposableSample._Fake.Verify(Times.Once, d => d.Dispose());
        }
    }
}
