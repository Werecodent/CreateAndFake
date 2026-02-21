using System.Reflection;
using CreateAndFake.AsserterTool;
using CreateAndFake.Design.Reiteration;
using CreateAndFake.FakerTool;
using CreateAndFake.RunnerTool;
using CreateAndFake.Samples.ErrorCases;
using CreateAndFake.Samples.Scenarios;
using CreateAndFake.TesterTool;
using CreateAndFake.Tests.TesterTool.TestSamples;

namespace CreateAndFake.Tests.TesterTool;

public static class MutationGuarderTests
{
    private static readonly TesterMod config = opt =>
        opt with
        {
            IncludeInternals = false,
            IgnorableExceptions =
            [
                typeof(InvalidOperationException),
                typeof(TargetParameterCountException),
                typeof(TargetException),
                typeof(ArgumentException),
                typeof(FormatException),
            ],
        };

    private static readonly MutationGuarder _ShortTestInstance = new(
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

    private static readonly MutationGuarder _LongTestInstance = new(
        Tools.Tester.Options with
        {
            Runner = new Runner(Tools.Runner.Options with { Timeout = new TimeSpan(0, 0, 10) }),
        }
    );

    [Fact]
    internal static Task MutationGuarder_GuardsNulls()
    {
        return Tools.Tester.PreventsNullRefExceptionAsync(
            _ShortTestInstance,
            TestContext.Current.CancellationToken,
            config
        );
    }

    [Fact]
    internal static Task PreventsParameterMutation_OnStatics()
    {
        return Tools
            .Tester.Assert(t =>
                t.PreventsParameterMutationAsync(
                    typeof(StaticMutationSample),
                    TestContext.Current.CancellationToken
                )
            )
            .Throws<AssertException>();
    }

    [Fact]
    internal static Task PreventsParameterMutation_StatelessFine()
    {
        return Tools.Tester.PreventsParameterMutationAsync<StatelessSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static Task PreventsParameterMutation_InjectsMultipleValues(
        Fake<IOnlyMockSample> fake1,
        Fake<IOnlyMockSample> fake2
    )
    {
        return Tools.Tester.PreventsParameterMutationAsync<InjectMockSample>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [fake1, fake2] }
        );
    }

    [Theory, RandomData]
    internal static Task PreventsParameterMutation_InjectsWithMethods(Fake<IOnlyMockSample> fake)
    {
        return Tools.Tester.PreventsParameterMutationAsync<MockMethodPassOnly>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [fake] }
        );
    }

    [Fact]
    internal static Task CallMethod_TimesOut()
    {
        return Limiter.Few.RetryAsync<AssertException>(
            "Attempting to test timeout works.",
            _ShortTestInstance
                .Assert(t =>
                    t.PreventsMutationOnStaticsAsync(
                        typeof(LongMethodSample),
                        false,
                        TestContext.Current.CancellationToken
                    )
                )
                .Throws<TimeoutException>(),
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static async Task PreventsMutationOnConstructors_Disposes()
    {
        await MockDisposableSample._Lock.WaitAsync(TestContext.Current.CancellationToken);
        try
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = Tools.Faker.Stub<IDisposable>();

            await _LongTestInstance.PreventsMutationOnConstructorsAsync(
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

    [Fact]
    internal static async Task PreventsMutationOnMethods_Disposes()
    {
        await MockDisposableSample._Lock.WaitAsync(TestContext.Current.CancellationToken);
        try
        {
            MockDisposableSample._ClassDisposes = 0;
            MockDisposableSample._FinalizerDisposes = 0;
            MockDisposableSample._Fake = Tools.Faker.Stub<IDisposable>();

            using MockDisposableSample sample = new(null);
            await _LongTestInstance.PreventsMutationOnMethodsAsync(
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
