using Werecodent.CreateAndFake.AsserterTool;
using Werecodent.CreateAndFake.Design.Reiteration;
using Werecodent.CreateAndFake.FakerTool;
using Werecodent.CreateAndFake.RunnerTool;
using Werecodent.CreateAndFake.Samples.ErrorCases;
using Werecodent.CreateAndFake.Samples.Scenarios;
using Werecodent.CreateAndFake.TesterTool.Guarders;
using Werecodent.CreateAndFake.Tests.TesterTool.TestSamples;

namespace Werecodent.CreateAndFake.Tests.TesterTool.Guarders;

public static class MutationGuarderTests
{
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
    internal static Task PreventsMutationOnStaticsAsync_UsesStatics()
    {
        return Tools
            .Tester.PreventsParameterMutationAsync(
                typeof(StaticMutationSample),
                TestContext.Current.CancellationToken,
                opt =>
                    opt with
                    {
                        DisableParameterMutationTests = false,
                        IncludeStaticMethods = true,
                    }
            )
            .Assert()
            .ThrowsAsync<AssertException>(TestContext.Current.CancellationToken);
    }

    [Fact]
    internal static Task PreventsMutationAsync_StatelessFine()
    {
        return Tools.Tester.PreventsParameterMutationAsync<StatelessSample>(
            TestContext.Current.CancellationToken
        );
    }

    [Theory, RandomData]
    internal static Task PreventsMutationOnMethodsAsync_InjectsMultipleValues(
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
    internal static Task PreventsMutationOnMethodsAsync_InjectsWithMethods(
        Fake<IOnlyMockSample> fake
    )
    {
        return Tools.Tester.PreventsParameterMutationAsync<MockMethodPassOnly>(
            TestContext.Current.CancellationToken,
            opt => opt with { InjectionValues = [fake] }
        );
    }

    [Fact]
    internal static Task PreventsMutationAsync_TimesOut()
    {
        return Limiter.Few.RetryAsync<AssertException>(
            "Attempting to test timeout works.",
            () =>
            {
                _ = _ShortTestInstance
                    .PreventsMutationOnStaticsAsync(
                        typeof(LongMethodSample),
                        false,
                        TestContext.Current.CancellationToken
                    )
                    .Assert()
                    .ThrowsAsync<TimeoutException>(TestContext.Current.CancellationToken);
            },
            TestContext.Current.CancellationToken
        );
    }

    [Fact]
    internal static async Task PreventsMutationOnConstructorsAsync_Disposes()
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
    internal static async Task PreventsMutationOnMethodsAsync_Disposes()
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
