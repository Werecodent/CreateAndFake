using CreateAndFake.AsserterTool;
using CreateAndFake.FakerTool;
using CreateAndFake.TesterTool;
using CreateAndFake.Tests.TesterTool.TestSamples;
using CreateAndFake.Tests.TestSamples;

namespace CreateAndFake.Tests.TesterTool;

public static class NullGuarderTests
{
    private static readonly NullGuarder _ShortTestInstance = new(
        Tools.Tester.Options with
        {
            Timeout = new TimeSpan(0, 0, 0, 0, 100),
        }
    );

    private static readonly NullGuarder _LongTestInstance = new(
        Tools.Tester.Options with
        {
            Timeout = new TimeSpan(0, 0, 10),
        }
    );

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
            .Assert(t =>
                t.PreventsNullRefExceptionOnConstructors(typeof(NullReferenceSample), true)
            )
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static void PreventsNullRefException_InjectsMultipleValues(
        Fake<IOnlyMockSample> fake1,
        Fake<IOnlyMockSample> fake2
    )
    {
        Tools.Tester.PreventsNullRefException<InjectMockSample>(opt =>
            opt with
            {
                InjectionValues = [fake1, fake2],
            }
        );
    }

    [Theory, RandomData]
    internal static void PreventsNullRefException_InjectsWithMethods(Fake<IOnlyMockSample> fake)
    {
        Tools.Tester.PreventsNullRefException<MockMethodPassOnly>(opt =>
            opt with
            {
                InjectionValues = [fake],
            }
        );
    }

    [Fact]
    internal static void PreventsNullRefException_OnStatics()
    {
        Tools
            .Tester.Assert(t => t.PreventsNullRefException(typeof(StaticMutationSample)))
            .Throws<AssertException>();
    }

    [Fact]
    internal static void PreventsNullRefException_StatelessFine()
    {
        Tools.Tester.PreventsNullRefException<StatelessSample>();
    }

    [Theory, RandomData]
    internal static void PreventsNullRefExceptionOnConstructors_Disposes(
        [Stub] IDisposable disposable
    )
    {
        lock (MockDisposableSample._Lock)
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = disposable.ToFake();

            _LongTestInstance.PreventsNullRefExceptionOnConstructors(
                typeof(MockDisposableSample),
                true
            );
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
