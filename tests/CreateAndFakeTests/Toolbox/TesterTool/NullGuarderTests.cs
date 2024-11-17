using CreateAndFake.Toolbox.AsserterTool;
using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.TesterTool;
using CreateAndFakeTests.TestSamples;
using CreateAndFakeTests.Toolbox.TesterTool.TestSamples;

namespace CreateAndFakeTests.Toolbox.TesterTool;

public static class NullGuarderTests
{
    private static readonly NullGuarder _ShortTestInstance = new(
        new GenericFixer(Tools.Tester.Options),
        Tools.Randomizer, Tools.Asserter, new TimeSpan(0, 0, 0, 0, 100));

    private static readonly NullGuarder _LongTestInstance = new(
        new GenericFixer(Tools.Tester.Options),
        Tools.Randomizer, Tools.Asserter, new TimeSpan(0, 0, 10));

    [Fact]
    internal static void NullGuarder_GuardsNulls()
    {
        Tools.Tester.PreventsNullRefException(_ShortTestInstance);
    }

    [Fact]
    internal static void NullGuarder_NoParameterMutation()
    {
        Tools.Tester.PreventsParameterMutation(_ShortTestInstance);
    }

    [Fact]
    internal static void NullCheck_TimesOut()
    {
        _ShortTestInstance
            .Assert(t => t.PreventsNullRefExceptionOnStatics(typeof(LongMethodSample), false))
            .Throws<TimeoutException>();
    }

    [Fact]
    internal static void NullCheck_NullReferenceThrows()
    {
        _ShortTestInstance
            .Assert(t => t.PreventsNullRefExceptionOnConstructors(typeof(NullReferenceSample), true))
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void PreventsNullRefException_InjectsMultipleValues(
        Fake<IOnlyMockSample> fake1, Fake<IOnlyMockSample> fake2)
    {
        Tools.Tester.PreventsNullRefException<InjectMockSample>(fake1, fake2);
    }

    [Theory, RandomData]
    internal static void PreventsNullRefException_InjectsWithMethods(Fake<IOnlyMockSample> fake)
    {
        Tools.Tester.PreventsNullRefException<MockMethodPassOnly>(fake);
    }

    [Fact]
    internal static void PreventsNullRefException_OnStatics()
    {
        Tools.Tester
            .Assert(t => t.PreventsNullRefException(typeof(StaticMutationSample)))
            .Throws<AssertException>();
    }

    [Fact]
    internal static void PreventsNullRefException_StatelessFine()
    {
        Tools.Tester.PreventsNullRefException<StatelessSample>();
    }

    [Fact]
    internal static void PreventsNullRefException_NullInjectionsFine()
    {
        Tools.Tester.PreventsNullRefException<StatelessSample>((object[])null);
    }

    [Theory, RandomData]
    internal static void PreventsNullRefExceptionOnConstructors_Disposes([Stub] IDisposable disposable)
    {
        lock (MockDisposableSample._Lock)
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = disposable.ToFake();

            _LongTestInstance.PreventsNullRefExceptionOnConstructors(typeof(MockDisposableSample), true);
            Tools.Asserter.Is(1, MockDisposableSample._ClassDisposes);
            Tools.Asserter.Is(0, MockDisposableSample._FinalizerDisposes);
            MockDisposableSample._Fake.Verify(Times.Once, d => d.Dispose());
        }
    }

    [Theory, RandomData]
    internal static void PreventsNullRefExceptionOnMethods_Disposes([Stub] IDisposable disposable)
    {
        lock (MockDisposableSample._Lock)
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = disposable.ToFake();

            using MockDisposableSample sample = new(null);
            _LongTestInstance.PreventsNullRefExceptionOnMethods(sample);
            Tools.Asserter.Is(0, MockDisposableSample._ClassDisposes);
            Tools.Asserter.Is(0, MockDisposableSample._FinalizerDisposes);
            MockDisposableSample._Fake.Verify(Times.Once, d => d.Dispose());
        }
    }
}
