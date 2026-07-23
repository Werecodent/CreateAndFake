using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Exceptions;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.Samples.ErrorCases;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.TesterTool.Guarders;
using CreateAndFake.Tests.TesterTool.TestSamples;

namespace CreateAndFake.Tests.TesterTool.Guarders;

public static class NullGuarderTests
{
    private static readonly TesterMod _Config = opt =>
        opt with
        {
            IgnoreAllExceptions = true,
            IncludeInternals = false,
        };

    private static readonly NullGuarder _ShortTestInstance = new(
        Tools.Tester.Options with
        {
            Runner = new Runner(
                Tools.Runner.Options with
                {
                    Timeout = new TimeSpan(0, 0, 0, 0, 100),
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
        return Tools.Tester.PreventsNullRefExceptionAsync(
            _ShortTestInstance,
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task NullGuarder_NoParameterMutation()
    {
        return Tools.Tester.PreventsParameterMutationAsync(
            _ShortTestInstance,
            TestContext.Current.CancellationToken,
            _Config
        );
    }

    [Fact]
    internal static Task PreventsNullRefExceptionAsync_TimesOut()
    {
        return _ShortTestInstance
            .PreventsNullRefExceptionOnStaticsAsync(
                typeof(LongMethodSample),
                false,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<RunnerTimeoutException>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static Task PreventsNullRefExceptionAsync_NullReferenceThrows()
    {
        return _ShortTestInstance
            .PreventsNullRefExceptionOnConstructorsAsync(
                typeof(NullReferenceSample),
                true,
                TestContext.Current.CancellationToken
            )
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Theory, RandomData]
    internal static Task PreventsNullRefExceptionAsync_InjectsMultipleValues(
        Fake<IOnlyMockSample> fake1,
        Fake<IOnlyMockSample> fake2
    )
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<InjectMockSample>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [fake1, fake2] }
        );
    }

    [Theory, RandomData]
    internal static Task PreventsNullRefExceptionAsync_InjectsWithMethods(
        Fake<IOnlyMockSample> fake
    )
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<MockMethodPassOnly>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [fake] }
        );
    }

    [Fact]
    internal static Task PreventsNullRefExceptionAsync_OnStatics()
    {
        return Tools
            .Tester.PreventsNullRefExceptionAsync(
                typeof(StaticMutationSample),
                TestContext.Current.CancellationToken,
                opt =>
                    opt with
                    {
                        DisableNullRefExceptionTests = false,
                        IncludeStaticMethods = true,
                    }
            )
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static Task PreventsNullRefExceptionAsync_StatelessFine()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync<StatelessSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static async Task PreventsNullRefExceptionOnConstructorsAsync_Disposes(
        [Stub] IDisposable disposable
    )
    {
        await MockDisposableSample._Lock.WaitAsync(TestContext.Current.CancellationToken);
        try
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = disposable.Tools().ToFake();

            await _LongTestInstance.PreventsNullRefExceptionOnConstructorsAsync(
                typeof(MockDisposableSample),
                true,
                TestContext.Current.CancellationToken
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
    internal static async Task PreventsNullRefExceptionOnMethodsAsync_Disposes(
        [Stub] IDisposable disposable
    )
    {
        await MockDisposableSample._Lock.WaitAsync(TestContext.Current.CancellationToken);
        try
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = disposable.Tools().ToFake();

            using MockDisposableSample sample = new(null);
            await _LongTestInstance.PreventsNullRefExceptionOnMethodsAsync(
                sample,
                TestContext.Current.CancellationToken
            );
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
