using CreateAndFake.AsserterTool;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.Samples.ErrorCases;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.TesterTool;
using CreateAndFake.Tests.TesterTool.TestSamples;

namespace CreateAndFake.Tests.TesterTool;

public static class NullGuarderTests
{
    private static readonly TesterMod config = opt => opt with { IgnoreAllExceptions = true };

    private static readonly NullGuarder _ShortTestInstance = new(
        Tools.Tester.Options with
        {
            Runner = new Runner(
                Tools.Runner.Options with
                {
                    Timeout = new TimeSpan(0, 0, 0, 0, 20),
                }
            ),
        }
    );

    private static readonly NullGuarder _LongTestInstance = new(
        Tools.Tester.Options with
        {
            Runner = new Runner(Tools.Runner.Options with { Timeout = new TimeSpan(0, 0, 15) }),
        }
    );

    [Fact]
    internal static Task NullGuarder_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefException(
            _ShortTestInstance,
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task NullGuarder_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutation(
            _ShortTestInstance,
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task NullCheck_TimesOut()
    {
        return _ShortTestInstance
            .Assert(t => t.PreventsNullRefExceptionOnStatics(typeof(LongMethodSample), false))
            .Throws<TimeoutException>();
    }

    [Fact]
    internal static Task NullCheck_NullReferenceThrows()
    {
        return _ShortTestInstance
            .Assert(t =>
                t.PreventsNullRefExceptionOnConstructors(typeof(NullReferenceSample), true)
            )
            .Throws<AssertException>();
    }

    [Theory, RandomData]
    internal static Task PreventsNullRefException_InjectsMultipleValues(
        Fake<IOnlyMockSample> fake1,
        Fake<IOnlyMockSample> fake2
    )
    {
        return Tools.Tester.PreventsNullRefException<InjectMockSample>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [fake1, fake2] }
        );
    }

    [Theory, RandomData]
    internal static Task PreventsNullRefException_InjectsWithMethods(Fake<IOnlyMockSample> fake)
    {
        return Tools.Tester.PreventsNullRefException<MockMethodPassOnly>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [fake] }
        );
    }

    [Fact]
    internal static Task PreventsNullRefException_OnStatics()
    {
        return Tools
            .Tester.Assert(t =>
                t.PreventsNullRefException(
                    typeof(StaticMutationSample),
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<AssertException>();
    }

    [Fact]
    internal static Task PreventsNullRefException_StatelessFine()
    {
        return Tools.Tester.PreventsNullRefException<StatelessSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static async Task PreventsNullRefExceptionOnConstructors_Disposes(
        [Stub] IDisposable disposable
    )
    {
        await MockDisposableSample._Lock.WaitAsync(TestContext.Current.CancellationToken);
        try
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = disposable.ToFake();

            await _LongTestInstance.PreventsNullRefExceptionOnConstructors(
                typeof(MockDisposableSample),
                true
            );
            Tools.Asserter.Is(1, MockDisposableSample._ClassDisposes);
            Tools.Asserter.Is(0, MockDisposableSample._FinalizerDisposes);
            MockDisposableSample._Fake.Verify(Times.Once, d => d.Dispose());
        }
        finally
        {
            MockDisposableSample._Lock.Release();
        }
    }

    [Theory, RandomData]
    internal static async Task PreventsNullRefExceptionOnMethods_Disposes(
        [Stub] IDisposable disposable
    )
    {
        await MockDisposableSample._Lock.WaitAsync(TestContext.Current.CancellationToken);
        try
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = disposable.ToFake();

            using MockDisposableSample sample = new(null);
            await _LongTestInstance.PreventsNullRefExceptionOnMethods(sample);
            Tools.Asserter.Is(0, MockDisposableSample._ClassDisposes);
            Tools.Asserter.Is(0, MockDisposableSample._FinalizerDisposes);
            MockDisposableSample._Fake.Verify(Times.Once, d => d.Dispose());
        }
        finally
        {
            MockDisposableSample._Lock.Release();
        }
    }
}
